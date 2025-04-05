using System.Security.Claims;
using System.Text.Json;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Auth.Dtos;
using Application.Identity;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Implementations;

public class TokenService(
    IUserRepository userRepository,
    ITokenGenerator tokenGenerator,
    ITokenRepository tokenRepository,
    IIdentityUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IOptionsMonitor<JwtOptions> optionsMonitor) : ITokenService
{
    private readonly JwtOptions _jwtOptions = optionsMonitor.CurrentValue;
    
    public async Task<TokensDto> GetTokens(AppUser appUser, bool rememberMe)
    {
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Email == appUser.Email);
        if (user is null)
        {
            throw new BusinessException(ErrorMessages.NotFoundUser);
        }
        
        var claims = await GetClaimsAsync(user, appUser);
        var accessToken = tokenGenerator.GenerateAccessToken(claims);
        RefreshToken? refreshToken = null;
        if (rememberMe)
        {
            refreshToken = tokenGenerator.GenerateRefreshToken(user.Id, appUser.Id);
            refreshToken.User = appUser;
            await tokenRepository.AddAsync(refreshToken);
        }
        
        await RemoveOldRefreshTokens(appUser);
        await unitOfWork.SaveChangesAsync();
        return new TokensDto(accessToken, refreshToken?.Token);
    }

    public async Task<TokensDto> RefreshTokenAsync(string token)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenWithUserByTokenAsync(token);

        if (refreshToken is null)
            throw new ArgumentValidationException(ErrorMessages.RefreshTokenNotFound);
                
        if (refreshToken.IsRevoked)
        {
            await RevokeDescendantRefreshTokens(refreshToken, refreshToken.User, $"Attempted reuse of revoked ancestor token: {token}");
            await unitOfWork.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
            throw new ArgumentValidationException(ErrorMessages.NotActiveRefreshToken);
        
        var newRefreshToken = RotateRefreshToken(refreshToken);
        await tokenRepository.AddAsync(newRefreshToken);
        
        await RemoveOldRefreshTokens(refreshToken.User);

        await unitOfWork.SaveChangesAsync();
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == refreshToken.UserId);
        if (user is null)
            throw new BusinessException(ErrorMessages.NotFoundUser);
        
        var jwtToken = tokenGenerator.GenerateAccessToken(await GetClaimsAsync(user, refreshToken.User));

        return new TokensDto(jwtToken, newRefreshToken.Token);
    }
    
    public void RevokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
    
    public string GetTwoFactorTokenProvider(AppUser appUser)
    {
        return appUser.TwoFactorType switch
        {
            TwoFactorType.Email => "Email",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private RefreshToken RotateRefreshToken(RefreshToken refreshToken)
    {
        var newRefreshToken = tokenGenerator.GenerateRefreshToken(refreshToken.UserId, refreshToken.AppUserId);
        newRefreshToken.User = refreshToken.User;
        newRefreshToken.UserId = refreshToken.UserId;
        RevokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
    
    private async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, AppUser user, string reason)
    {
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;
        
        var childToken = (await tokenRepository.GetRefreshTokensAsync(user))
            .SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken is { IsActive: true })
            RevokeRefreshToken(childToken, reason);
        else if (childToken != null) 
            await RevokeDescendantRefreshTokens(childToken, user, reason);
    }
    
    private async Task RemoveOldRefreshTokens(AppUser user)
    {
        await tokenRepository.RemoveAllRefreshTokensAsync(user, x =>
            !x.IsActive &&
            x.Created.AddDays(_jwtOptions.RefreshTokenLifetimeInDays) <= DateTime.UtcNow);
    }
    
    private async Task<List<Claim>> GetClaimsAsync(User user, AppUser appUser)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Nickname),
            new("subscribeId", JsonSerializer.Serialize(
                user.UserSubscriptions!
                    .Where(x => x.ExpiresAt < DateTimeOffset.Now 
                                && x.Status == UserSubscriptionStatus.Completed)
                    .Select(x => x.SubscriptionId).ToList()))
        };
        
        var roles = await userManager.GetRolesAsync(appUser);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}