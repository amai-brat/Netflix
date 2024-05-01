using Domain.Entities;

namespace Infrastructure.Identity.Data;

public interface ITokenRepository
{
    public Task<List<RefreshToken>> GetRefreshTokensAsync(AppUser user);
    public Task RemoveAllRefreshTokensAsync(AppUser user, Func<RefreshToken, bool> predicate);
    public Task<RefreshToken?> GetRefreshTokenWithUserByTokenAsync(string token);
    public Task AddAsync(RefreshToken refreshToken);
}