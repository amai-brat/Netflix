using System.Security.Claims;
using Infrastructure.Identity;

namespace Infrastructure.Services.Abstractions;

public interface ITokenGenerator
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    public RefreshToken GenerateRefreshToken(long userId, long appUserId);
}