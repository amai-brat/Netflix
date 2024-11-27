using System.Security.Claims;

namespace Application.Identity;

public interface ITokenGenerator
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    public RefreshToken GenerateRefreshToken(long userId, long appUserId);
}