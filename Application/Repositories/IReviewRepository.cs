using System.Linq.Expressions;
using Application.Dto;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter);
        Task AssignReviewAsync(Review review);
        Task<List<Review>> GetByReviewSearchDtoAsync(ReviewSearchDto dto, int reviewsPerPage);
        Task<int> GetPagesCountAsync(ReviewSearchDto dto, int reviewsPerPage);
        Task<int?> GetScoreByUserAsync(long userId, long contentId);
    }
}
