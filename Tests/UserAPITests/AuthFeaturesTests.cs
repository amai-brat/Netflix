using System.Linq.Expressions;
using System.Security.Claims;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Auth.Commands.ChangeEmail;
using Application.Features.Auth.Commands.ChangeRole;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.SignIn;
using Application.Identity;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoFixture;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Tests.UserAPITests.Fakes;

namespace Tests.UserAPITests;

public class AuthFeaturesTests
{
    private UserManager<AppUser>? _userManager;
    private SignInManager<AppUser>? _signInManager;
    private readonly Mock<IUserStore<AppUser>> _mockUserStore = new();
    private readonly Mock<IUserRepository> _mockUserRepo = new();
    private readonly Mock<IUnitOfWork> _mockAppUnitOfWork = new();
    private readonly Mock<IIdentityUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<ITokenGenerator> _mockTokenGenerator = new();
    private readonly Mock<ITokenRepository> _mockTokenRepo = new();
    private readonly Mock<ITokenService> _mockTokenService = new();
    private readonly Mock<IEmailSender> _mockEmailSeder = new();
    private readonly Mock<ITwoFactorTokenSender> _mockTwoFactorTokenSender = new();
    private readonly Mock<IOptionsMonitor<JwtOptions>> _mockMonitor = new();
    private readonly Mock<ISubscriptionRepository> _mockSubscriptions = new();
    private readonly Fixture _fixture = new();
    private readonly IServiceProvider _serviceProvider;

    public AuthFeaturesTests()
    {
        _serviceProvider = new TestServiceProviderBuilder()
            .With(services =>
            {
                services.AddScoped<IUserStore<AppUser>>(_ => _mockUserStore.Object);
                services.AddScoped<IUserRepository>(_ => _mockUserRepo.Object);
                services.AddScoped<IUnitOfWork>(_ => _mockAppUnitOfWork.Object);
                services.AddScoped<IIdentityUnitOfWork>(_ => _mockUnitOfWork.Object);
                services.AddScoped<ITokenGenerator>(_ => _mockTokenGenerator.Object);
                services.AddScoped<ITokenService>(_ => _mockTokenService.Object);
                services.AddScoped<ITokenRepository>(_ => _mockTokenRepo.Object);
                services.AddScoped<ITwoFactorTokenSender>(_ => _mockTwoFactorTokenSender.Object);
                services.AddScoped<IEmailSender>(_ => _mockEmailSeder.Object);
                services.AddScoped<IOptionsMonitor<JwtOptions>>(_ => _mockMonitor.Object);
                services.AddScoped<ISubscriptionRepository>(_ => _mockSubscriptions.Object);
                services.AddScoped<UserManager<AppUser>>(_ => _userManager!);
                services.AddScoped<SignInManager<AppUser>>(_ => _signInManager!);

                services.AddHttpContextAccessor();
                services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            })
            .Build();
    }

    [Fact]
    public async Task ChangeRole_CorrectRoleGiven_EmailReturned()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        Setup(users, appUsers);
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var newRole = "moderator";
        
        var result = await mediator.Send(new ChangeRoleCommand(new UserRoleDto
        {
            UserId = 1, 
            Role = newRole
        }));

        // assert
        Assert.NotEmpty(result.Email);
    }
    
    [Fact]
    public async Task ChangeEmail_EmailGiven_DomainUsersEmailChanged()
    {
        // arrange
        var users = GetUsers();
        var appUsers = GetAppUsers();
        Setup(users, appUsers);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        await mediator.Send(new ChangeEmailCommand(1, "kek@cock.li", "aboba"));
        
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
        
        Setup(users, appUsers);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act 
        await mediator.Send(new RegisterCommand(dto));
        
        // assert
        _mockUserRepo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        Assert.Contains(users, x => x.Email == dto.Email);
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
        Setup(users, appUsers);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var ex = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(new SignInCommand(dto)));
        
        // assert
        Assert.Contains(ErrorMessages.IncorrectPassword, ex.Message);
    }

    private void Setup(List<User> users, List<AppUser> appUsers)
    {
        SetupMocks(users, appUsers);
        _userManager = new FakeUserManager(appUsers);
        _signInManager = new FakeSignInManager((_userManager as FakeUserManager)!);
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

    private void SetupMocks(List<User> users, List<AppUser> _)
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