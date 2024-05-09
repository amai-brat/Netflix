using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Data;

public class TokenRepository(IdentityDbContext dbContext) : ITokenRepository
{
    public async Task<List<RefreshToken>> GetRefreshTokensAsync(AppUser user)
    {
        return await dbContext.RefreshTokens
            .Where(token => token.AppUserId == user.Id)
            .ToListAsync();
    }

    public async Task RemoveAllRefreshTokensAsync(AppUser user, Func<RefreshToken, bool> predicate)
    {
        var tokens = dbContext.RefreshTokens
            .Where(x => x.AppUserId == user.Id)
            .AsAsyncEnumerable();
        await foreach (var token in tokens)
        {
            if (predicate(token))
            {
                dbContext.RefreshTokens.Remove(token);
            }
        }
    }

    public async Task<RefreshToken?> GetRefreshTokenWithUserByTokenAsync(string token)
    {
        return await dbContext.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Token == token);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken);
    }
}