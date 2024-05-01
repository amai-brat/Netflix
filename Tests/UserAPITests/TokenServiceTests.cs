using Application.Options;
using Application.Repositories;
using Application.Services.Implementations;
using AutoFixture;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.UserAPITests;

public class TokenServiceTests
{
    private readonly Mock<ITokenRepository> _mockTokenRepo = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IOptionsMonitor<JwtOptions>> _mockMonitor = new();
    private readonly Fixture _fixture = new();
    
    [Fact]
    public async Task GenerateTokens_UserGiven_OldRefreshTokensDeleted()
    {
        // arrange
        var user = new User { Id = 1, Role = "user" };
        var tokens = _fixture.Build<RefreshToken>()
            .With(x => x.UserId, 1)
            .With(x => x.Created, DateTime.UtcNow.AddDays(-4))
            .With(x => x.Expires, DateTime.UtcNow.AddDays(-3))
            .Without(x => x.User)
            .CreateMany(5)
            .ToList();

        _mockMonitor.Setup(x => x.CurrentValue)
            .Returns(new JwtOptions
            {
                Key = "AccessTokenLifetimeInMinutesABIBA",
                AccessTokenLifetimeInMinutes = 228,
                RefreshTokenLifetimeInDays = 1
            });
        
        _mockTokenRepo.Setup(x => x.RemoveAllRefreshTokensAsync(It.IsAny<User>(), It.IsAny<Func<RefreshToken, bool>>()))
            .Returns((User u, Func<RefreshToken, bool> pr) =>
            {
                tokens.RemoveAll(t => t.UserId == u.Id && pr(t));
                return Task.CompletedTask;
            });
        
        var service = GetTokenService();
        
        // act
        await service.GenerateTokensAsync(TODO, user);
        
        // assert
        Assert.True(tokens.All(x => x.IsActive && x.Expires > DateTime.Now.AddDays(-_mockMonitor.Object.CurrentValue.RefreshTokenLifetimeInDays)));
    }

    [Fact]
    public async Task RevokeToken_TokenGiven_Revoked()
    {
        // arrange
        var user = new User { Id = 1, Role = "user"};
        DateTime? dateTime = null;
        var tokens = _fixture.Build<RefreshToken>()
            .With(x => x.UserId, 1)
            .With(x => x.Revoked, dateTime)
            .With(x => x.Expires, DateTime.UtcNow.AddDays(1))
            .Without(x => x.User)
            .CreateMany(5)
            .ToList();

        _mockMonitor.Setup(x => x.CurrentValue)
            .Returns(new JwtOptions
            {
                Key = "AccessTokenLifetimeInMinutesABIBA",
                AccessTokenLifetimeInMinutes = 228,
                RefreshTokenLifetimeInDays = 1
            });
        
        _mockTokenRepo.Setup(x => x.RemoveAllRefreshTokensAsync(It.IsAny<User>(), It.IsAny<Func<RefreshToken, bool>>()))
            .Returns((User u, Func<RefreshToken, bool> pr) =>
            {
                tokens.RemoveAll(t => t.UserId == u.Id && pr(t));
                return Task.CompletedTask;
            });
        _mockTokenRepo.Setup(x => x.GetRefreshTokenWithUserByTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string t) => tokens
                .First(x => x.Token == t));
        
        var service = GetTokenService();
        
        // act
        await service.RevokeTokenAsync(tokens.First().Token);
        
        // assert
        Assert.True(tokens.First().IsRevoked);
    }

    [Fact]
    public async Task RefreshToken_OldTokenGiven_OldRevokedAndNewReturned()
    {
        // assert
        var user = new User { Id = 1, Role = "user"};
        DateTime? dateTime = null;
        var i = 1;
        var tokens = _fixture.Build<RefreshToken>()
            .With(x => x.Id, () => i++)
            .With(x => x.UserId, 1)
            .With(x => x.User, user)
            .With(x => x.Revoked, dateTime)
            .With(x => x.Created, DateTime.UtcNow.AddDays(1))
            .With(x => x.Expires, DateTime.UtcNow.AddDays(2))
            .Without(x => x.User)
            .CreateMany(5)
            .ToList();
        
        _mockTokenRepo.Setup(x => x.RemoveAllRefreshTokensAsync(It.IsAny<User>(), It.IsAny<Func<RefreshToken, bool>>()))
            .Returns((User u, Func<RefreshToken, bool> pr) =>
            {
                tokens.RemoveAll(t => t.UserId == u.Id && pr(t));
                return Task.CompletedTask;
            });
        _mockTokenRepo.Setup(x => x.GetRefreshTokenWithUserByTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((string t) => tokens
                .First(x => x.Token == t));
        _mockMonitor.Setup(x => x.CurrentValue)
            .Returns(new JwtOptions
            {
                Key = "AccessTokenLifetimeInMinutesABIBA",
                AccessTokenLifetimeInMinutes = 228,
                RefreshTokenLifetimeInDays = 1
            });
        
        var service = GetTokenService();
        
        // act;
        await service.RefreshTokenAsync(tokens.First(x => x.Id == 1).Token);
        
        // assert
        Assert.True(tokens.First(x => x.Id == 1).IsRevoked);
    }
    
    private TokenService GetTokenService()
    {
        return new TokenService(_mockTokenRepo.Object, _mockUnitOfWork.Object, _mockMonitor.Object);
    }
}