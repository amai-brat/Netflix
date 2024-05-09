using System.Linq.Expressions;
using Application.Dto;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using Infrastructure.Profiles;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Moq;

namespace Tests.UserAPITests;

public class UserServiceTests
{
    public UserServiceTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ReviewProfile());
            mc.AddProfile(new FavouriteProfile());
        });
        _mapper = mappingConfig.CreateMapper();
    }

    private readonly Fixture _fixture = new();
    private readonly Mock<IUserRepository> _mockUserRepo = new();
    private readonly Mock<IProfilePicturesProvider> _mockPictureProvider = new();
    private readonly Mock<IFavouriteContentRepository> _mockFavouirteRepo = new();
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IReviewRepository> _mockReviewRepo = new();
    
    [Fact]
    public async Task AllMethods_UserNotFound_ErrorReturned()
    {
        // arrange
        var users = GetUsers();
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var userService = GetUserService();
        
        // act 
        const int notFoundId = -1;
        
        var exPersonalInfo = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.GetPersonalInfoAsync(notFoundId));
        var exBirthday = await Assert.ThrowsAsync<UserServiceArgumentException>(async () => 
            await userService.ChangeBirthdayAsync(notFoundId, DateOnly.FromDateTime(DateTime.Now).AddDays(-1)));
        var exPicture = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.ChangeProfilePictureAsync(notFoundId, new MemoryStream(), "image/png"));
        var exFavourites = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.GetFavouritesAsync(notFoundId));

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

        
        var userService = GetUserService();
        
        // act 
        var result = await userService.GetPersonalInfoAsync(1);
        
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
        
        var service = GetUserService();
        
        // act
        var exFuture = await Assert.ThrowsAsync<UserServiceArgumentException>(async() => 
            await service.ChangeBirthdayAsync(1, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))));
        var exPast = await Assert.ThrowsAsync<UserServiceArgumentException>(async () => 
            await service.ChangeBirthdayAsync(1, DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-160))));
        
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
        
        var service = GetUserService();
        var newBirthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18));
        
        // act
        var result = await service.ChangeBirthdayAsync(1, newBirthday);
        
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
        
        var service = GetUserService();
        
        // act
        var result = await service.GetReviewsAsync(searchDto);
        
        // assert
        Assert.True(result.Count > 0);
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
        
        var service = GetUserService();
        
        // act
        var result = await service.GetReviewsPagesCountAsync(searchDto);
        
        // assert
        Assert.True(result >= 0);
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
        
        var service = GetUserService();
        
        // act
        var result = await service.GetFavouritesAsync(1);
        
        // assert
        foreach (var dto in result)
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
        
        var service = GetUserService();
        
        // act
        await service.ChangeProfilePictureAsync(1, new MemoryStream(), "image/png");
        
        // assert
        Assert.True(users.Single(x => x.Id == 1).ProfilePictureUrl != previousPicture);
    }
    
    private UserService GetUserService()
    {
        return new UserService(_mockPictureProvider.Object, _mockFavouirteRepo.Object, _mockUserRepo.Object, _mapper, _mockReviewRepo.Object, _mockUnitOfWork.Object);
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