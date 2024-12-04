using System.Linq.Expressions;
using Application.Dto;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Users.Commands.ChangeBirthday;
using Application.Features.Users.Commands.ChangeProfilePicture;
using Application.Features.Users.Queries.GetFavourites;
using Application.Features.Users.Queries.GetPersonalInfo;
using Application.Features.Users.Queries.GetReviews;
using Application.Features.Users.Queries.GetReviewsPagesCount;
using Application.Providers;
using Application.Repositories;
using AutoFixture;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.UserAPITests;

public class UserFeaturesTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IUserRepository> _mockUserRepo = new();
    private readonly Mock<IProfilePicturesProvider> _mockPictureProvider = new();
    private readonly Mock<IFavouriteContentRepository> _mockFavouirteRepo = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IReviewRepository> _mockReviewRepo = new();
    private readonly IServiceProvider _serviceProvider;
    
    public UserFeaturesTests()
    {
        _serviceProvider = new TestServiceProviderBuilder()
            .With(services =>
            {
                services.AddScoped<IUserRepository>(_ => _mockUserRepo.Object);
                services.AddScoped<IProfilePicturesProvider>(_ => _mockPictureProvider.Object);
                services.AddScoped<IFavouriteContentRepository>(_ => _mockFavouirteRepo.Object);
                services.AddScoped<IUnitOfWork>(_ => _mockUnitOfWork.Object);
                services.AddScoped<IReviewRepository>(_ => _mockReviewRepo.Object);
                
                services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            })
            .Build();
    }
    
    [Fact]
    public async Task AllMethods_UserNotFound_ErrorReturned()
    {
        // arrange
        var users = GetUsers();
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act 
        const int notFoundId = -1;
        
        var exPersonalInfo = await Assert.ThrowsAsync<ArgumentValidationException>(async () =>
            await mediator.Send(new GetPersonalInfoQuery(notFoundId)));
        var exBirthday = await Assert.ThrowsAsync<ArgumentValidationException>(async () => 
            await mediator.Send(new ChangeBirthdayCommand(notFoundId, DateOnly.FromDateTime(DateTime.Now).AddDays(-1))));
        var exPicture = await Assert.ThrowsAsync<ArgumentValidationException>(async () =>
            await mediator.Send(new ChangeProfilePictureCommand(notFoundId, new MemoryStream(), "image/png")));
        var exFavourites = await Assert.ThrowsAsync<ArgumentValidationException>(async () => 
            await mediator.Send(new GetFavouritesQuery(notFoundId)));

        // assert
        Assert.Contains(ErrorMessages.NotFoundUser, exPersonalInfo.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exBirthday.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exPicture.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exFavourites.Message);
    }
    
    [Fact]
    public async Task GetPersonalInfo_UserExists_DtoReturned()
    {
        // arrange
        var users = GetUsers();
        users.Single(x => x.Id == 1).BirthDay = DateOnly.ParseExact("12.01.2024", "dd.MM.yyyy");
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockUserRepo.Setup(x => x.GetUserWithSubscriptionsAsync(It.IsAny<Expression<Func<User,bool>>>()))
            .ReturnsAsync((Expression<Func<User,bool>> exp) => users.Single(exp.Compile()));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;

        // act 
        var result = await mediator.Send(new GetPersonalInfoQuery(1));
        
        // assert
        var user = users.Single(x => x.Id == 1);
        Assert.True(result.Email == user.Email &&
                    result.BirthDay == "12.01.2024" &&
                    result.Nickname == user.Nickname);
    }

    [Fact]
    public async Task ChangeBirthday_DayInFutureOrDayInVeryPastGiven_ErrorReturned()
    {
        // arrange
        var users = GetUsers();

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exFuture = await Assert.ThrowsAsync<ArgumentValidationException>(async() => 
            await mediator.Send(new ChangeBirthdayCommand(1, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))));
        var exPast = await Assert.ThrowsAsync<ArgumentValidationException>(async () => 
            await mediator.Send(new ChangeBirthdayCommand(1, DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-160)))));
        
        // assert
        Assert.Contains(ErrorMessages.InvalidBirthday, exFuture.Message);
        Assert.Contains(ErrorMessages.InvalidBirthday, exPast.Message);
    }
    
    [Fact]
    public async Task ChangeBirthday_AvailableBirthdayGiven_EntityChanged()
    {
        // arrange
        var users = GetUsers();

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        var newBirthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18));
        
        // act
        var result = await mediator.Send(new ChangeBirthdayCommand(1, newBirthday));
        
        // assert
        
        Assert.True(result.BirthDay == newBirthday);
    }

    [Fact]
    public async Task GetReviews_DtoGiven_ReviewsReturned()
    {
        // arrange
        var searchDto = new ReviewSearchDto
        {
            Page = 0,
            UserId = 1,
            Search = ""
        };

        var users = GetUsers();
        var i = 1;
        var reviews = _fixture.Build<Review>()
            .With(x => x.Id, () => i++)
            .With(x => x.UserId, 1)
            .With(x => x.Text)
            .OmitAutoProperties()
            .CreateMany(25);

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockReviewRepo.Setup(x => x.GetByReviewSearchDtoAsync(It.IsAny<ReviewSearchDto>(), It.IsAny<int>()))
            .ReturnsAsync((ReviewSearchDto dto, int reviewPerPage) => reviews
                .Where(x => x.UserId == 1)
                .Where(x => x.Text.Contains(dto.Search ?? ""))
                .Take(reviewPerPage)
                .ToList());
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var result = await mediator.Send(new GetReviewsQuery(searchDto));
        
        // assert
        Assert.True(result.ReviewDtos.Count > 0);
    }

    [Fact]
    public async Task GetReviewsPagesCount_DtoGiven_NumberMoreOrEqualZeroReturned()
    {
        // arrange
        var searchDto = new ReviewSearchDto
        {
            Page = 0,
            UserId = 1,
            Search = ""
        };

        var users = GetUsers();
        var i = 1;
        var reviews = _fixture.Build<Review>()
            .With(x => x.Id, () => i++)
            .With(x => x.UserId, 1)
            .With(x => x.Text)
            .OmitAutoProperties()
            .CreateMany(25);

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockReviewRepo.Setup(x => x.GetPagesCountAsync(It.IsAny<ReviewSearchDto>(), It.IsAny<int>()))
            .ReturnsAsync((ReviewSearchDto dto, int reviewPerPage) => (int)Math.Ceiling(reviews
                .Where(x => x.UserId == 1)
                .Count(x => x.Text.Contains(dto.Search ?? "")) 
                / (double)reviewPerPage));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var result = await mediator.Send(new GetReviewsPagesCountQuery(searchDto));
        
        // assert
        Assert.True(result.Count >= 0);
    }

    [Fact]
    public async Task GetFavourites_UserIdGiven_FavouritesReturned()
    {
        // arrange
        var users = GetUsers();
        var content = new ContentBase
        {
            Id = 1,
            Name = "AAA",
            PosterUrl = "aab",
        };
        
        var reviews = _fixture.Build<Review>()
            .With(x => x.Score, () => Random.Shared.Next(1, 11))
            .With(x => x.UserId, 1)
            .With(x => x.ContentId, () => 1)
            .OmitAutoProperties()
            .CreateMany(5)
            .ToList();
        var favourites = _fixture.Build<FavouriteContent>()
            .With(x => x.UserId, 1)
            .With(x => x.AddedAt, DateTimeOffset.Now)
            .With(x => x.ContentId, 1)
            .With(x => x.Content, content)
            .OmitAutoProperties()
            .CreateMany(10)
            .ToList();

        _mockReviewRepo.Setup(x => x.GetScoreByUserAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync((long userId, long contentId) => reviews.First(x => x.UserId == userId && x.ContentId == contentId).Score);
        _mockFavouirteRepo.Setup(x => x.GetWithContentAsync(It.IsAny<Expression<Func<FavouriteContent, bool>>>()))
            .ReturnsAsync((Expression<Func<FavouriteContent, bool>> filter) => favourites.Where(filter.Compile()).ToList());
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var result = await mediator.Send(new GetFavouritesQuery(1));
        
        // assert
        foreach (var dto in result.FavouriteDtos)
        {
            Assert.Equal(reviews.First(x => x.ContentId == dto.ContentBase.Id).Score, dto.Score );
        }
    }

    [Fact]
    public async Task ChangeProfilePicture_PictureGiven_EntityChanged()
    {
        // arrange
        var users = GetUsers();
        var previousPicture = users.Single(x => x.Id == 1).ProfilePictureUrl;
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockPictureProvider.Setup(x => x.PutAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
            .Returns((string _, Stream _, string _) => Task.CompletedTask);
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        await mediator.Send(new ChangeProfilePictureCommand(1, new MemoryStream(), "image/png"));
        
        // assert
        Assert.True(users.Single(x => x.Id == 1).ProfilePictureUrl != previousPicture);
    }

    private List<User> GetUsers()
    {
        var i = 1;
        return _fixture.Build<User>()
            .With(x => x.Id, () => i++)
            .With(x => x.Email)
            .With(x => x.Nickname)
            .With(x => x.ProfilePictureUrl)
            .OmitAutoProperties()
            .CreateMany(5)
            .ToList();
    }
}