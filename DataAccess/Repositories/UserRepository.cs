using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter) =>
            await appDbContext.Users.SingleOrDefaultAsync(filter);
    }
}
