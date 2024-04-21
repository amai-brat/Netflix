using Application.Dto;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Implementations
{
    public class ReviewService(
        IReviewRepository reviewRepository,
        IContentRepository contentRepository,
        IUserRepository userRepository,
        IMapper mapper
        ) : IReviewService
    {
        public async Task AssignReviewWithRatingAsync(ReviewAssignDto review, long userId)
        {
            if(!IsValidReview(review, out var errorMessage, out var param))
                throw new ReviewServiceArgumentException(errorMessage!, param!);

            if (await contentRepository.GetContentByFilterAsync(c => c.Id == review.ContentId) is null)
                throw new ReviewServiceArgumentException(ErrorMessages.NotFoundContent, $"{review.ContentId}");

            if (await userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
                throw new ReviewServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

            await reviewRepository.AssignReviewAsync(new Review()
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
            await reviewRepository.GetReviewsByFilterAsync(r => r.ContentId == contentId);

		public async Task<List<Review>> GetReviewsByContentIdAsync(long contentId, string sort) => 
			((sort.ToLower(), await GetReviewsByContentIdAsync(contentId)) switch
			{
				("score", var reviews) => reviews.OrderBy(r => r.Score),
				("scoredesc", var reviews) => reviews.OrderByDescending(r => r.Score),
				("oldest", var reviews) => reviews.OrderBy(r => r.WrittenAt),
				("newest", var reviews) => reviews.OrderByDescending(r => r.WrittenAt),
				("positive", var reviews) => reviews.OrderBy(r => r.IsPositive),
				("negative", var reviews) => reviews.OrderByDescending(r => r.IsPositive),
				("likes", var reviews) => reviews.OrderBy(r => r.RatedByUsers?.Count(usersReviews => usersReviews.IsLiked) ?? 0),
				("likesdesc", var reviews) => reviews.OrderByDescending(r => r.RatedByUsers?.Count(usersReviews => usersReviews.IsLiked) ?? 0),
				var (_, _) => throw new ReviewServiceArgumentException(ErrorMessages.IncorrectSortType, sort)
			}).ToList();

		public async Task<List<ReviewDto>> GetReviewsByContentIdAsync(long contentId, string sort, int offset, int limit)
		{
			if (offset < 0 || limit < 0)
				throw new ReviewServiceArgumentException(ErrorMessages.ArgumentsMustBePositive, $"offset = {offset}; limit = {limit}");

			var reviews = await GetReviewsByContentIdAsync(contentId, sort);
			var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);
			return reviewDtos[Math.Min(reviews.Count, offset)..Math.Min(reviews.Count, offset + limit)];
		}

		public async Task<Review> DeleteReviewByIdAsync(long id)
		{
			var review = await reviewRepository.GetReviewByIdAsync(id);

			if (review == null)
			{
				throw new ReviewServiceArgumentException(ErrorMessages.NotFoundReview, nameof(id));
			}

			var deletedReview = reviewRepository.DeleteReview(review);
			await reviewRepository.SaveChangesAsync();

			return deletedReview;
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
