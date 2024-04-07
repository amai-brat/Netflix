using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

namespace DataAccess.Repositories
{
    public class ReviewRepository(AppDbContext appDbContext) : IReviewRepository
    {
        public async Task AssignReviewAsync(Review review)
        {
            await appDbContext.AddAsync(review);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<List<Review>> GetByReviewSearchDto(ReviewSearchDto dto, int reviewsPerPage)
        {
            return await appDbContext.Reviews
                .Include(x => x.Content)
                .Where(x => x.UserId == dto.UserId)
                .Where(x => x.Text.Contains(dto.Search ?? ""))
                .Skip(dto.Page * reviewsPerPage)
                .Take(reviewsPerPage)
                .OrderByDescending(x => 
                    dto.SortType == ReviewSortType.Rating 
                        ? x.Score 
                        : dto.SortType == ReviewSortType.DateUpdated 
                            ? x.WrittenAt.ToUnixTimeMilliseconds()
                            : x.Id)
                .ToListAsync();
        }

        public async Task<int> GetPagesCountAsync(ReviewSearchDto dto, int reviewsPerPage)
        {
            var pages = await appDbContext.Reviews
                .Where(x => x.UserId == dto.UserId)
                .Where(x => x.Text.Contains(dto.Search ?? ""))
                .CountAsync() / reviewsPerPage;
            return pages <= 0 
                ? 1
                : pages;
        }

        public async Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter) =>
            await appDbContext.Reviews.Where(filter)
                .Include(r => r.User)
                .Include(r => r.Comments)
                    .ThenInclude(r => r.User)
                 .Include(r => r.Comments)
                    .ThenInclude(r => r.ScoredByUsers)
                .Include(r => r.RatedByUsers)
                .ToListAsync();
    }
}
