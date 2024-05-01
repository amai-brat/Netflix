using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.Dto;
using Application.Exceptions;
using Application.Options;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Implementations;

public class TokenService(
    ITokenRepository tokenRepository,
    IUnitOfWork unitOfWork,
    IOptionsMonitor<JwtOptions> monitor) : ITokenService
{
    private readonly JwtOptions _jwtOptions = monitor.CurrentValue;
    
    private string GenerateAccessToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("subscribeId", JsonSerializer.Serialize(user.UserSubscriptions?.Select(x => x.SubscriptionId).ToList()))
            }),
            
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(User user)
    {
        return new RefreshToken
        {
            UserId = user.Id,
            User = user,
            Token = RandomNumberGenerator.GetHexString(32),
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeInDays),
            Created = DateTime.UtcNow,
        };
    }

    public async Task<TokensDto> GenerateTokensAsync(User user, bool withRefreshToken = true)
    {
        RefreshToken? refreshToken = null;
        if (withRefreshToken)
        {
            refreshToken = GenerateRefreshToken(user);
            await tokenRepository.AddAsync(refreshToken);
        }
        
        await RemoveOldRefreshTokens(user);
        await unitOfWork.SaveChangesAsync();

        var accessToken = GenerateAccessToken(user);
        
        return new TokensDto(accessToken, refreshToken?.Token);
    }

    public async Task<TokensDto> RefreshTokenAsync(string token)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenWithUserByTokenAsync(token);

        if (refreshToken is null)
            throw new TokenServiceArgumentException(ErrorMessages.RefreshTokenNotFound, nameof(token));
        
        
        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            await RevokeDescendantRefreshTokens(refreshToken, refreshToken.User, $"Attempted reuse of revoked ancestor token: {token}");

            await unitOfWork.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
            throw new TokenServiceArgumentException(ErrorMessages.NotActiveRefreshToken, nameof(token));
        

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = RotateRefreshToken(refreshToken);
        await tokenRepository.AddAsync(newRefreshToken);

        // remove old refresh tokens from user
        await RemoveOldRefreshTokens(refreshToken.User);

        await unitOfWork.SaveChangesAsync();

        // generate new jwt
        var jwtToken = GenerateAccessToken(newRefreshToken.User);

        return new TokensDto(jwtToken, newRefreshToken.Token);
    }

    public async Task RevokeTokenAsync(string token)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenWithUserByTokenAsync(token);

        if (refreshToken is null)
            throw new TokenServiceArgumentException(ErrorMessages.RefreshTokenNotFound, nameof(token));

        if (!refreshToken.IsActive)
            throw new TokenServiceArgumentException(ErrorMessages.NotActiveRefreshToken, nameof(token));

        // revoke token and save
        RevokeRefreshToken(refreshToken, "Revoked without replacement");

        await unitOfWork.SaveChangesAsync();
    }
    
    private async Task RemoveOldRefreshTokens(User user)
    {
        // remove old inactive refresh tokens from user based on TTL in app settings
        await tokenRepository.RemoveAllRefreshTokensAsync(user, x =>
            !x.IsActive &&
            x.Created.AddDays(_jwtOptions.RefreshTokenLifetimeInDays) <= DateTime.UtcNow);
    }
    
    private async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;
        
        var childToken = (await tokenRepository.GetRefreshTokensAsync(user))
            .SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken is { IsActive: true })
            RevokeRefreshToken(childToken, reason);
        else if (childToken != null) 
            await RevokeDescendantRefreshTokens(childToken, user, reason);
    }
    
    private static void RevokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
    
    private RefreshToken RotateRefreshToken(RefreshToken refreshToken)
    {
        var newRefreshToken = GenerateRefreshToken(refreshToken.User);
        newRefreshToken.User = refreshToken.User;
        newRefreshToken.UserId = refreshToken.UserId;
        RevokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
}