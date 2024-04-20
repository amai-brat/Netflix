using System.Linq.Expressions;
using Domain.Entities;

namespace DataAccess.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter);
    }
}
