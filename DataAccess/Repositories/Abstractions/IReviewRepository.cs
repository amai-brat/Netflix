using System.Linq.Expressions;
using Domain.Entities;

namespace DataAccess.Repositories.Abstractions
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter);
        Task AssignReviewAsync(Review review);
    }
}
