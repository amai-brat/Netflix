using System.Linq.Expressions;
using Application.Dto;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewByFilterAsync(Expression<Func<Review, bool>> filter);
        Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter);
        Task AssignReviewAsync(Review review);
        Task<int> GetReviewsCountAsync(long contentId);
        Task<List<Review>> GetByReviewSearchDtoAsync(ReviewSearchDto dto, int reviewsPerPage);
        Task<int> GetPagesCountAsync(ReviewSearchDto dto, int reviewsPerPage);
        Task<int?> GetScoreByUserAsync(long userId, long contentId);
        Task<Review?> GetReviewByIdAsync(long id);
        Review DeleteReview(Review review);
        Task<bool> IsReviewLikedByUserAsync(long reviewId, long userId);
        Task RemoveReviewLikeAsync(long reviewId, long userId);
        Task AddReviewLikeAsync(long reviewId, long userId);
        Task SaveChangesAsync();
    }
}
