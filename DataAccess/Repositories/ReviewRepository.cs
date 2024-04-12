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
    public class ReviewRepository(AppDbContext appDbContext) : IReviewRepository
    {
        public async Task AssignReviewAsync(Review review)
        {
            await appDbContext.AddAsync(review);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Review?> GetReviewByFilterAsync(Expression<Func<Review, bool>> filter) =>
            await appDbContext.Reviews.SingleOrDefaultAsync(filter);

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
