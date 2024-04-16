﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Repositories;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter) =>
            await appDbContext.Users.SingleOrDefaultAsync(filter);

        public async Task<User?> AddAsync(User user)
        {
            var entry = await appDbContext.Users.AddAsync(user);
            return entry.Entity;
        }
    }
}
