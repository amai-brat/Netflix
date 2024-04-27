using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Repositories;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<User?> GetUserWithSubscriptionsAndRolesByFilterAsync(Expression<Func<User, bool>> filter)
        {
            return await appDbContext.Users
                .Include(u => u.UserSubscriptions)
                .Where(filter).SingleOrDefaultAsync();
        }

        public async Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter) =>
            await appDbContext.Users.SingleOrDefaultAsync(filter);

        public async Task<User?> GetUserWithSubscriptionsAsync(long userId)
        {
            return await appDbContext.Users
                .Include(x => x.UserSubscriptions)
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync();
        }

        public async Task<User?> AddAsync(User user)
        {
            var entry = await appDbContext.Users.AddAsync(user);
            return entry.Entity;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await appDbContext.Users.AllAsync(x => x.Email != email);
        }
    }
}
