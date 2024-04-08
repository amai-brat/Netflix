using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;

namespace Domain.Abstractions
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByFilterAsync(Expression<Func<Review, bool>> filter);
        Task AssignReviewAsync(Review review);
        Task<List<Review>> GetByReviewSearchDto(ReviewSearchDto dto, int reviewsPerPage);
        Task<int> GetPagesCountAsync(ReviewSearchDto dto, int reviewsPerPage);
        Task<int?> GetScoreByUserAsync(long userId, long contentId);

    }
}
