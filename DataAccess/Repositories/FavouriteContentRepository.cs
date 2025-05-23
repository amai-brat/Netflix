﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Repositories;

namespace DataAccess.Repositories
{
    public class FavouriteContentRepository (AppDbContext appDbContext): IFavouriteContentRepository
    {
        public async Task AddFavouriteContentAsync(long contentId, long userId)
        {
            await appDbContext.FavouriteContents.AddAsync(new FavouriteContent() 
            {
                ContentId = contentId,
                UserId = userId,
                AddedAt = DateTimeOffset.UtcNow
            });
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<FavouriteContent>> GetFavouriteContentsByFilterAsync(Expression<Func<FavouriteContent, bool>> filter) => 
            await appDbContext.FavouriteContents.Where(filter)
                .ToListAsync();

        public async Task RemoveFavouriteContentAsync(long contentId, long userId)
        {
            var favouriteContent = (await GetFavouriteContentsByFilterAsync(f => f.UserId == userId && f.ContentId == contentId)).Single();
            appDbContext.FavouriteContents.Remove(favouriteContent);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<FavouriteContent>> GetWithContentAsync(Expression<Func<FavouriteContent, bool>> filter)
        {
            return await appDbContext.FavouriteContents
                .Include(x => x.Content)
                .Where(filter)
                .ToListAsync();
        }
    }
}
