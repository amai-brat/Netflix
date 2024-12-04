using Application.Cache;
using Application.Dto;
using Application.Exceptions.ErrorMessages;
using Application.Exceptions.Particular;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Implementations
{
	[Obsolete("CQRS")]
    public class ReviewService(
        IReviewRepository reviewRepository,
        IContentRepository contentRepository,
        IUserRepository userRepository,
        IMinioCache minioCache,
        IUserService userService,
        IMapper mapper
        ) : IReviewService
    {
	    public async Task<int> GetReviewsCountByContentIdAsync(long contentId)
	    {
		    return await reviewRepository.GetReviewsCountAsync(contentId);
	    }

		public async Task<bool> LikeReviewAsync(long reviewId, long userId)
		{
			if (!await reviewRepository.IsReviewLikedByUserAsync(reviewId, userId))
			{
				await reviewRepository.AddReviewLikeAsync(reviewId, userId);
				await reviewRepository.SaveChangesAsync();
				return true;
			}

			await reviewRepository.RemoveReviewLikeAsync(reviewId, userId);
			await reviewRepository.SaveChangesAsync();

			return false;
		}

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
			
			var content = await contentRepository.GetContentByFilterAsync(c => c.Id == review.ContentId);
			var reviewCount = await reviewRepository.GetReviewsCountAsync(review.ContentId);
			if (content!.Ratings == null)
			{
				content.Ratings = new Ratings();
			}

            content
				.Ratings
				.LocalRating =
				((content.Ratings.LocalRating ?? 0) * (reviewCount-1) + review.Score!.Value)
				/ (reviewCount);

            // format float local rating to 2 decimal places
            content.Ratings.LocalRating = (float) Math.Round(content.Ratings.LocalRating.Value, 2);

			await contentRepository.SaveChangesAsync();
		}

		public async Task AssignReviewAsync(ReviewAssignDto review, long userId)
		{
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
				("positive", var reviews) => reviews.OrderByDescending(r => r.IsPositive),
				("negative", var reviews) => reviews.OrderBy(r => r.IsPositive),
				("likes", var reviews) => reviews.OrderBy(r => r.RatedByUsers?.Count(usersReviews => usersReviews.IsLiked) ?? 0),
				("likesdesc", var reviews) => reviews.OrderByDescending(r => r.RatedByUsers?.Count(usersReviews => usersReviews.IsLiked) ?? 0),
				var (_, _) => throw new ReviewServiceArgumentException(ErrorMessages.IncorrectSortType, sort)
			}).ToList();

		public async Task<List<ReviewDto>> GetReviewsByContentIdAsync(long contentId, string sort, int offset, int limit)
		{
			if (offset < 0 || limit < 0)
				throw new ReviewServiceArgumentException(ErrorMessages.ArgumentsMustBePositive, $"offset = {offset}; limit = {limit}");

			var reviews = await GetReviewsByContentIdAsync(contentId, sort);
			// current user profile pic url are just guid strings. they must be converted into presigned urls
			// but it would be kind of expensive so we'll cache it guid-url to redis for 1 hour

			foreach (var review in reviews)
			{
				if (review.User.ProfilePictureUrl == null)
				{
					continue;
				}

				var picture = await minioCache.GetStringAsync(review.User.ProfilePictureUrl);
				if (picture == null)
				{
					// это случай если картинка из oauth. тогда ее не кешируем и не преобразуем в url
					if (review.User.ProfilePictureUrl.StartsWith("http"))
					{
						continue;
					}
					review.User.ProfilePictureUrl = await userService.ConvertProfilePictureGuidToUrlAsync(review.User.ProfilePictureUrl);
					await minioCache.SetStringAsync(review.User.ProfilePictureUrl, review.User.ProfilePictureUrl);
				}

				if (picture != null)
				{
					review.User.ProfilePictureUrl = picture;
				}
			}
			
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

			//Обновляем оценки
			var contentId = deletedReview.ContentId;
			var content = await contentRepository.GetContentByIdAsync(contentId);
            var reviewCount = await reviewRepository.GetReviewsCountAsync(contentId);

			content!.Ratings!.LocalRating =
				reviewCount == 0 
				? 0
				: (content.Ratings.LocalRating * reviewCount - deletedReview.Score) / (reviewCount - 1);
			
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
