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
    private readonly IPasswordHasher _passwordHasher = new PasswordHasher();
    private readonly Mock<ITokenService> _mockTokenService = new();
    
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
        var exEmail = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.ChangeEmailAsync(notFoundId, "a@.a"));
        var exBirthday = await Assert.ThrowsAsync<UserServiceArgumentException>(async () => 
            await userService.ChangeBirthdayAsync(notFoundId, DateOnly.FromDateTime(DateTime.Now).AddDays(-1)));
        var exPassword = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.ChangePasswordAsync(notFoundId, new ChangePasswordDto()));
        var exPicture = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.ChangeProfilePictureAsync(notFoundId, new MemoryStream(), "image/png"));
        var exFavourites = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.GetFavouritesAsync(notFoundId));
        var exRole = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
           await userService.ChangeRoleAsync(notFoundId, "admin"));
        var exAuthenticate = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await userService.AuthenticateAsync(new LoginDto{Email = "a", Password = "Qwe123!@#", RememberMe = true}));

        // assert
        Assert.Contains(ErrorMessages.NotFoundUser, exPersonalInfo.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exEmail.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exBirthday.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exPicture.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exFavourites.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exPassword.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exRole.Message);
        Assert.Contains(ErrorMessages.NotFoundUser, exAuthenticate.Message);
    }
    
    [Fact]
    public async Task GetPersonalInfo_UserExists_DtoReturned()
    {
        // arrange
        var users = GetUsers();
        users.Single(x => x.Id == 1).BirthDay = DateOnly.ParseExact("12.01.2024", "dd.MM.yyyy");
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
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
    public async Task ChangeRole_CorrectRoleGiven_EntityChanged()
    {
        // arrange
        var users = GetUsers();

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));

        var service = GetUserService();

        // act
        var newRole = "moderator";
        await service.ChangeRoleAsync(1, newRole);

        // assert
        Assert.True(users.Single(x => x.Id == 1).Role == newRole);
    }

    [Fact]
    public async Task ChangeRole_InvalidRoleGiven_ErrorReturned()
    {
        // arrange
        var users = GetUsers();

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));

        var service = GetUserService();

        // act
        var ex = await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await service.ChangeRoleAsync(1, "godRole"));

        // assert
        Assert.Contains(ErrorMessages.IncorrectRole, ex.Message);
    }

   [Fact]
    public async Task ChangeEmail_CorrectEmailGiven_EntityChanged()
    {
        // arrange
        var users = GetUsers();

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var service = GetUserService();
        
        // act
        await service.ChangeEmailAsync(1, "kek@cock.li");
        
        // assert
        Assert.True(users.Single(x => x.Id == 1).Email == "kek@cock.li");
    }

    [Fact]
    public async Task ChangeEmail_InvalidEmailGiven_ErrorReturned()
    {
        // arrange
        var users = GetUsers();

        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var service = GetUserService();
        
        // act
        var ex = await Assert.ThrowsAsync<UserServiceArgumentException>(async () => 
            await service.ChangeEmailAsync(1, "k@"));
        
        // assert
        Assert.Contains(ErrorMessages.InvalidEmail, ex.Message);
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
    public async Task ChangePassword_InvalidPreviousPasswordGiven_ErrorReturned()
    {
        // arrange
        var users = GetUsers();
        users.Single(x => x.Id == 1).Password = "uselessmouth";
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var service = GetUserService();
        
        // act
        var ex = await Assert.ThrowsAsync<UserServiceArgumentException>(async () => 
            await service.ChangePasswordAsync(1, new ChangePasswordDto {PreviousPassword = "aboba", NewPassword = "Qwe123!@#"})) ;
        
        // assert
        Assert.Contains(ErrorMessages.IncorrectPassword, ex.Message);
    }

    [Fact]
    public async Task ChangePassword_InvalidNewPasswordGiven_ErrorsReturned()
    {
        // arrange
        var users = GetUsers();
        users.Single(x => x.Id == 1).Password = _passwordHasher.Hash("uselessmouth");
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var service = GetUserService();
        
        // act
        await Assert.ThrowsAsync<UserServiceArgumentException>(async () =>
            await service.ChangePasswordAsync(1,
                new ChangePasswordDto { PreviousPassword = "uselessmouth", NewPassword = "aboba" }));
        
        // assert
        Assert.True(_passwordHasher.Verify("uselessmouth", users.Single(x => x.Id == 1).Password));
    }

    [Fact]
    public async Task ChangePassword_CorrectDtoGiven_EntityChanged()
    {
        // arrange
        var users = GetUsers();
        var prevPassword = "uselessmouth";
        users.Single(x => x.Id == 1).Password = _passwordHasher.Hash(prevPassword);
        
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        
        var service = GetUserService();
        
        // act
        await service.ChangePasswordAsync(1, new ChangePasswordDto {PreviousPassword = prevPassword, NewPassword = "Qwe123!@#"});
        
        // assert
        Assert.True(users.Single(x => x.Id == 1).Password != prevPassword);
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

    [Fact]
    public async Task Register_CorrectDtoGiven_NewUserIdReturned()
    {
        // arrange 
        var users = GetUsers();

        var dto = new SignUpDto
        {
            Login = "Aboba",
            Email = "a@a.a",
            Password = "Qwe123!@#"
        };

        _mockUserRepo.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => users.All(x => x.Email != email));
        _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) =>
            {
                u.Id = Random.Shared.NextInt64();
                users.Add(u);

                return u;
            });

        var service = GetUserService();
        
        // act 
        await service.RegisterAsync(dto);
        
        // assert
        _mockUserRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        Assert.Contains(users, x => x.Email == dto.Email);
    }

    [Fact]
    public async Task Register_NotUniqueEmailGiven_ErrorThrown()
    {
        // arrange 
        var users = GetUsers();
        users.First().Email = "a@a.a";

        var dto = new SignUpDto
        {
            Login = "Aboba",
            Email = "a@a.a",
            Password = "Qwe123!@#"
        };
        
        _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) =>
            {
                u.Id = Random.Shared.NextInt64();
                users.Add(u);

                return u;
            });

        var service = GetUserService();
        
        // act 
        var ex = await Assert.ThrowsAsync<UserServiceArgumentException>(
            async () => await service.RegisterAsync(dto));
        
        // assert
        _mockUserRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        Assert.Contains(ErrorMessages.EmailNotUnique, ex.Message);
    }
    
    [Fact]
    public async Task Authenticate_CorrectDtoGiven_TokensReturned()
    {
        // arrange 
        var users = GetUsers();
        users[0] = new User
        {
            Email = "a@a.a",
            Password = _passwordHasher.Hash("Qwe123!@#")
        };
        
        var dto = new LoginDto
        {
            Email = "a@a.a",
            Password = "Qwe123!@#"
        };
        
        _mockTokenService.Setup(x => x.GenerateTokensAsync(It.IsAny<User>(), It.IsAny<bool>()))
            .ReturnsAsync((User u, bool b) => new TokensDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));

        var service = GetUserService();
        
        // act
        var tokens = await service.AuthenticateAsync(dto);
        
        // assert
        Assert.NotNull(tokens);
    }
    
    [Fact]
    public async Task Authenticate_IncorrectPassword_ErrorThrown()
    {
        // arrange 
        var users = GetUsers();
        users[0] = new User
        {
            Email = "a@a.a",
            Password = _passwordHasher.Hash("Qwe123!@#")
        };
        
        var dto = new LoginDto
        {
            Email = "a@a.a",
            Password = "PassWord229*"
        };
        
        _mockTokenService.Setup(x => x.GenerateTokensAsync(It.IsAny<User>(), It.IsAny<bool>()))
            .ReturnsAsync((User u, bool b) => new TokensDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));

        var service = GetUserService();
        
        // act
        var ex = await Assert.ThrowsAsync<UserServiceArgumentException>(
            async () => await service.AuthenticateAsync(dto));
        
        // assert
        Assert.Contains(ErrorMessages.IncorrectPassword, ex.Message);
    }
    
    [Fact]
    public async Task RefreshToken_TokenGiven_NewTokenReturned()
    {
        // arrange
        const string token = "abiba";
        
        _mockTokenService.Setup(x => x.RefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string _) => new TokensDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        var service = GetUserService();
        
        // act
        var tokens = await service.RefreshTokenAsync(token);
        
        // assert
        Assert.NotEqual(token, tokens.RefreshToken);
    }
    
    private UserService GetUserService()
    {
        return new UserService(_mockPictureProvider.Object, _mockFavouirteRepo.Object, _mockUserRepo.Object, _mapper, _mockReviewRepo.Object, _mockUnitOfWork.Object, _passwordHasher, _mockTokenService.Object);
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