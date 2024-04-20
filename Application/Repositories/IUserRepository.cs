using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter);
        Task<User?> AddAsync(User user);
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
