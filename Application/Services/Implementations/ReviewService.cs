using Application.Dto;
using Application.Exceptions;
using Application.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace Application.Services.Implementations
{
    public class ReviewService(
        IReviewRepository reviewRepository,
        IContentRepository contentRepository,
        IUserRepository userRepository
        ) : IReviewService
    {
        private readonly IReviewRepository _reviewRepository = reviewRepository;
        private readonly IContentRepository _contentRepository = contentRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task AssignReviewWithRatingAsync(ReviewAssignDto review, long userId)
        {
            if(!IsValidReview(review, out var errorMessage, out var param))
                throw new ReviewServiceArgumentException(errorMessage!, param!);

            if (await _contentRepository.GetContentByFilterAsync(c => c.Id == review.ContentId) is null)
                throw new ReviewServiceArgumentException(ErrorMessages.NotFoundContent, $"{review.ContentId}");

            if (await _userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
                throw new ReviewServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

            await _reviewRepository.AssignReviewAsync(new Review()
            {
                UserId = userId,
                ContentId = review.ContentId,
                Text = review.Text,
                IsPositive = review.IsPositive,
                Score = review.Score ?? -1,
                WrittenAt = DateTimeOffset.UtcNow
            });
        }

        public async Task AssignReviewAsync(ReviewAssignDto review, long userId)
        {
            review.Score = null;
            await AssignReviewWithRatingAsync(review, userId);
        }

        public async Task<List<Review>> GetReviewsByContentIdAsync(long contentId) =>
            await _reviewRepository.GetReviewsByFilterAsync(r => r.ContentId == contentId);

        public async Task<List<Review>> GetReviewsByContentIdAsync(long contentId, string sort) => 
            ((sort.ToLower(), await GetReviewsByContentIdAsync(contentId)) switch
            {
                ("score", var reviews) => reviews.OrderBy(r => r.Score),
                ("scoredesc", var reviews) => reviews.OrderByDescending(r => r.Score),
                ("oldest", var reviews) => reviews.OrderBy(r => r.WrittenAt),
                ("newest", var reviews) => reviews.OrderByDescending(r => r.WrittenAt),
                ("positive", var reviews) => reviews.OrderBy(r => r.IsPositive),
                ("negative", var reviews) => reviews.OrderByDescending(r => r.IsPositive),
                ("likes", var reviews) => reviews.OrderBy(r => r.RatedByUsers?.Count(r => r.IsLiked) ?? 0),
                ("likesdesc", var reviews) => reviews.OrderByDescending(r => r.RatedByUsers?.Count(r => r.IsLiked) ?? 0),
                (_, var reviews) => throw new ReviewServiceArgumentException(ErrorMessages.IncorrectSortType, sort)
            }).ToList();

        public async Task<List<Review>> GetReviewsByContentIdAsync(long contentId, string sort, int offset, int limit)
        {
            if (offset < 0 || limit < 0)
                throw new ReviewServiceArgumentException(ErrorMessages.ArgumentsMustBePositive, $"offset = {offset}; limit = {limit}");

            var reviews = await GetReviewsByContentIdAsync(contentId, sort);
            return reviews[Math.Min(reviews.Count, offset)..Math.Min(reviews.Count, offset + limit)];
        }

        private bool IsValidReview(ReviewAssignDto review, out string? errorMessage, out string? param)
        {
            errorMessage = null;
            param = null;

            if (string.IsNullOrEmpty(review.Text))
            {
                errorMessage = ErrorMessages.ReviewMustHaveText;
                param = nameof(review.Text);
            }
            else if (review.Score is not null && (review.Score < 0 || review.Score > 10))
            {
                errorMessage = ErrorMessages.ScoreMustBeValid;
                param = nameof(review.Score);
            }

            return errorMessage == null;
        }
    }
}
