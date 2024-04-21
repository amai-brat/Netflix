using Application.Dto;
using Domain.Entities;

namespace Application.Services.Abstractions
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsByContentIdAsync(long contentId);
        Task<List<Review>> GetReviewsByContentIdAsync(long contentId, string sort);
        Task<List<ReviewDto>> GetReviewsByContentIdAsync(long contentId, string sort, int offset, int limit);
        Task AssignReviewWithRatingAsync(ReviewAssignDto review, long userId);
        Task AssignReviewAsync(ReviewAssignDto review, long userId);
        Task<Review> DeleteReviewByIdAsync(long id);
    }
}
