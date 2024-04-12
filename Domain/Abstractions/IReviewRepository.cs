using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewByFilterAsync(Expression<Func<Review, bool>> filter);
        Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter);
        Task AssignReviewAsync(Review review);
    }
}
