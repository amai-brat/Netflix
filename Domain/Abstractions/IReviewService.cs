using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsByContentIdAsync(long contentId);
        Task<List<Review>> GetReviewsByContentIdAsync(long contentId, string sort);
        Task<List<Review>> GetReviewsByContentIdAsync(long contentId, string sort, int offset, int limit);
        Task AssignReviewWithRatingAsync(ReviewAssignDto review, long userId);
        Task AssignReviewAsync(ReviewAssignDto review, long userId);
        Task<Review> DeleteReviewByIdAsync(long id);
    }
}
