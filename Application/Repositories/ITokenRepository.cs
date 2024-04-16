using Domain.Entities;

namespace Application.Repositories;

public interface ITokenRepository
{
    Task<List<RefreshToken>> GetRefreshTokensAsync(User user);
    Task RemoveAllRefreshTokensAsync(User user, Func<RefreshToken, bool> predicate);
    Task<RefreshToken?> GetRefreshTokenWithUserByTokenAsync(string token);
    Task AddAsync(RefreshToken refreshToken);
}