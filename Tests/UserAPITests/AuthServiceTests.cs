using System.Linq.Expressions;
using System.Security.Claims;
using Application.Dto;
using Application.Exceptions;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Options;
using Infrastructure.Profiles;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Exceptions;
using Infrastructure.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Tests.UserAPITests.Fakes;

namespace Tests.UserAPITests;

public class AuthServiceTests
{
    public AuthServiceTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new UserProfile());
        });
        _mapper = mappingConfig.CreateMapper();
    }

    private UserManager<AppUser>? _userManager;
    private SignInManager<AppUser>? _signInManager;
    private readonly Mock<IUserStore<AppUser>> _mockUserStore = new();
    private readonly Mock<IUserRepository> _mockUserRepo = new();
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockAppUnitOfWork = new();
    private readonly Mock<IIdentityUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<ITokenGenerator> _mockTokenGenerator = new();
    private readonly Mock<ITokenRepository> _mockTokenRepo = new();
    private readonly Mock<IEmailSender> _mockEmailSeder = new();
    private readonly Mock<ITwoFactorTokenSender> _mockTwoFactorTokenSender = new();
    private readonly Mock<IOptionsMonitor<JwtOptions>> _mockMonitor = new();
    private readonly Mock<ISubscriptionRepository> _mockSubscriptions = new();
    private readonly Fixture _fixture = new();
    
    [Fact]
    public async Task ChangeRole_CorrectRoleGiven_EmailReturned()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        var service = GetService(users, appUsers);

        // act
        var newRole = "moderator";
        var result = await service.ChangeRoleAsync(1, newRole);

        // assert
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task ChangeEmail_EmailGiven_DomainUsersEmailChanged()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        
        var service = GetService(users, appUsers);
        
        // act
        await service.ChangeEmailAsync(1, "kek@cock.li", "aboba");
        
        // assert
        Assert.True(users.Single(x => x.Id == 1).Email == "kek@cock.li");
    }
    
    [Fact]
    public async Task Register_CorrectDtoGiven_NewUserIdReturned()
    {
        // arrange 
        var users = GetUsers();
        var appUsers = GetAppUsers();
    
        var dto = new SignUpDto
        {
            Login = "Aboba",
            Email = "a@a.a",
            Password = "Qwe123!@#"
        };
    
        var service = GetService(users, appUsers);
        
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
        var appUsers = GetAppUsers();
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
    
        var service = GetService(users, appUsers);
        
        // act 
        var ex = await Assert.ThrowsAsync<AuthServiceException>(
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
        var appUsers = GetAppUsers();
        
        appUsers[0].EmailConfirmed = true;
        appUsers[0].TwoFactorEnabled = false;
        
        var dto = new LoginDto
        {
            Email = appUsers[0].Email!,
            Password = "1"
        };
        
        var service = GetService(users, appUsers);
        
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
        var appUsers = GetAppUsers();
        
        appUsers[0].EmailConfirmed = true;
        appUsers[0].TwoFactorEnabled = false;
        
        var dto = new LoginDto
        {
            Email = appUsers[0].Email!,
            Password = "PassWord229*"
        };
        
        var service = GetService(users, appUsers);
        
        // act
        var ex = await Assert.ThrowsAsync<AuthServiceException>(
            async () => await service.AuthenticateAsync(dto));
        
        // assert
        Assert.Contains(ErrorMessages.IncorrectPassword, ex.Message);
    }


    [Fact]
    public async Task Authenticate_DtoGiven_OldRefreshTokensDeleted()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        
        appUsers[0].EmailConfirmed = true;
        appUsers[0].TwoFactorEnabled = false;
        
        var dto = new LoginDto
        {
            Email = appUsers[0].Email!,
            Password = "1"
        };
        
        var tokens = _fixture.Build<RefreshToken>()
            .With(x => x.UserId, 1)
            .With(x => x.Created, DateTime.UtcNow.AddDays(-4))
            .With(x => x.Expires, DateTime.UtcNow.AddDays(-3))
            .Without(x => x.User)
            .CreateMany(5)
            .ToList();
        
        _mockTokenRepo.Setup(x => x.RemoveAllRefreshTokensAsync(It.IsAny<AppUser>(), It.IsAny<Func<RefreshToken, bool>>()))
            .Returns((AppUser u, Func<RefreshToken, bool> pr) =>
            {
                tokens.RemoveAll(t => t.UserId == u.Id && pr(t));
                return Task.CompletedTask;
            });

        var service = GetService(users, appUsers);

        // act
        await service.AuthenticateAsync(dto);

        // assert
        Assert.True(tokens.All(x =>
            x.IsActive && x.Expires >
            DateTime.Now.AddDays(-_mockMonitor.Object.CurrentValue.RefreshTokenLifetimeInDays)));
    }

    [Fact]
    public async Task RefreshToken_OldTokenGiven_OldRevokedAndNewReturned()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        DateTime? dateTime = null;
        var i = 1;
        var tokens = _fixture.Build<RefreshToken>()
            .With(x => x.Id, () => i++)
            .With(x => x.UserId, 1)
            .With(x => x.AppUserId, 1)
            .With(x => x.User, appUsers.First(x => x.Id == 1))
            .With(x => x.Revoked, dateTime)
            .With(x => x.Created, DateTime.UtcNow.AddDays(1))
            .With(x => x.Expires, DateTime.UtcNow.AddDays(2))
            .Without(x => x.User)
            .CreateMany(5)
            .ToList();

        _mockTokenGenerator.Setup(x => x.GenerateRefreshToken(It.IsAny<long>(), It.IsAny<long>()))
            .Returns((long u, long au) =>
                _fixture.Build<RefreshToken>()
                    .With(x => x.UserId, u)
                    .With(x => x.AppUserId, au)
                    .With(x => x.Created, DateTime.UtcNow)
                    .Create());
        _mockTokenRepo.Setup(x =>
                x.RemoveAllRefreshTokensAsync(It.IsAny<AppUser>(), It.IsAny<Func<RefreshToken, bool>>()))
            .Returns((AppUser u, Func<RefreshToken, bool> pr) =>
            {
                tokens.RemoveAll(t => t.UserId == u.Id && pr(t));
                return Task.CompletedTask;
            });
        _mockTokenRepo.Setup(x => x.GetRefreshTokenWithUserByTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string t) => tokens
                .First(x => x.Token == t));

        var service = GetService(users, appUsers);

        // act;
        await service.RefreshTokenAsync(tokens.First(x => x.Id == 1).Token);

        // assert
        Assert.True(tokens.First(x => x.Id == 1).IsRevoked);
    }

    [Fact]
    public async Task RevokeToken_TokenGiven_Revoked()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        DateTime? dateTime = null;
        var tokens = _fixture.Build<RefreshToken>()
            .With(x => x.UserId, 1)
            .With(x => x.Revoked, dateTime)
            .With(x => x.Expires, DateTime.UtcNow.AddDays(1))
            .Without(x => x.User)
            .CreateMany(5)
            .ToList();
        
        _mockTokenRepo.Setup(x => x.RemoveAllRefreshTokensAsync(It.IsAny<AppUser>(), It.IsAny<Func<RefreshToken, bool>>()))
            .Returns((AppUser u, Func<RefreshToken, bool> pr) =>
            {
                tokens.RemoveAll(t => t.UserId == u.Id && pr(t));
                return Task.CompletedTask;
            });
        _mockTokenRepo.Setup(x => x.GetRefreshTokenWithUserByTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string t) => tokens
                .First(x => x.Token == t));

        var service = GetService(users, appUsers);

        // act
        await service.RevokeTokenAsync(tokens.First().Token);

        // assert
        Assert.True(tokens.First().IsRevoked);
    }

    private AuthService GetService(List<User> users, List<AppUser> appUsers)
    {
        SetupMocks(users, appUsers);
        _userManager = new FakeUserManager(appUsers);
        _signInManager = new FakeSignInManager((_userManager as FakeUserManager)!);
        return new AuthService(
            _userManager, _signInManager, _mockUserRepo.Object, _mockSubscriptions.Object, _mapper, _mockAppUnitOfWork.Object,
            _mockUnitOfWork.Object, _mockTokenGenerator.Object, _mockTokenRepo.Object, _mockEmailSeder.Object,
            _mockTwoFactorTokenSender.Object, _mockMonitor.Object);

    }
    
    private List<User> GetUsers()
    {
        var i = 1;
        var email = 1;
        return _fixture.Build<User>()
            .With(x => x.Id, () => i++)
            .With(x => x.Email, () => (email++).ToString())
            .With(x => x.Nickname)
            .With(x => x.ProfilePictureUrl)
            .With(x => x.UserSubscriptions, [])
            .OmitAutoProperties()
            .CreateMany(5)
            .ToList();
    }

    private List<AppUser> GetAppUsers()
    {
        var i = 1;
        var email = 1;
        var password = 1;
        return _fixture.Build<AppUser>()
            .With(x => x.Id, () => i++)
            .With(x => x.Email, () => (email++).ToString())
            .With(x => x.PasswordHash, () => (password++).ToString())
            .CreateMany(5)
            .ToList();
    }

    private void SetupMocks(List<User> users, List<AppUser> appUsers)
    {
        _mockUserRepo.Setup(x => x.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> exp) => users.SingleOrDefault(exp.Compile()));
        _mockUserRepo.Setup(x => x.GetUserWithSubscriptionsAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> exp) => users.SingleOrDefault(exp.Compile()));
        _mockUserRepo.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => users.All(x => x.Email != email));
        _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) =>
            {
                u.Id = Random.Shared.NextInt64();
                users.Add(u);
    
                return u;
            });
        _mockSubscriptions.Setup(x => x.GetAllSubscriptionsAsync())
            .ReturnsAsync(() => new List<Subscription>());
        _mockMonitor.Setup(x => x.CurrentValue)
            .Returns(new JwtOptions
            {
                Key = "AccessTokenLifetimeInMinutesABIBA",
                AccessTokenLifetimeInMinutes = 228,
                RefreshTokenLifetimeInDays = 1
            });

        _mockTokenGenerator.Setup(x => x.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>()))
            .Returns((IEnumerable<Claim> _) => Guid.NewGuid().ToString());
    }
}