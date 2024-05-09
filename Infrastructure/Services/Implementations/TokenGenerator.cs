using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Options;
using Infrastructure.Identity;
using Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Implementations;

public class TokenGenerator(IOptionsMonitor<JwtOptions> monitor) : ITokenGenerator
{
    private readonly JwtOptions _jwtOptions = monitor.CurrentValue;
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public RefreshToken GenerateRefreshToken(long userId, long appUserId)
    {
        return new RefreshToken
        {
            UserId = userId,
            AppUserId = appUserId,
            Token = RandomNumberGenerator.GetHexString(32),
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeInDays),
            Created = DateTime.UtcNow,
        };
    }
}