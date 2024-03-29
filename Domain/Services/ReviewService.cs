using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ReviewService(
        IReviewRepository reviewRepository,
        IContentRepository contentRepository
        ) : IReviewService
    {
        private readonly IReviewRepository _reviewRepository = reviewRepository;
        private readonly IContentRepository _contentRepository = contentRepository;

        public async Task AssignReviewWithRatingAsync(ReviewAssignDto review, long userId)
        {
            if(!IsValidReview(review, out var errorMessage, out var param))
                throw new ReviewServiceArgumentException(errorMessage!, param!);

            if (await _contentRepository.GetContentByFilterAsync(c => c.Id == review.ContentId) is null)
                throw new ReviewServiceArgumentException(ErrorMessages.NotFoundContent, $"{review.ContentId}");

            await _reviewRepository.AssignReviewAsync(new Review()
            {
                UserId = userId,
                ContentId = review.ContentId,
                Text = review.Text,
                IsPositive = review.IsPositive,
                Score = review.Score,
                WrittenAt = DateTimeOffset.UtcNow
            });
        }

        public async Task AssignReviewAsync(ReviewAssignDto review, long userId)
        {
            if (!IsValidReview(review, out var errorMessage, out var param))
                throw new ReviewServiceArgumentException(errorMessage!, param!);

            if (await _contentRepository.GetContentByFilterAsync(c => c.Id == review.ContentId) is null)
                throw new ReviewServiceArgumentException(ErrorMessages.NotFoundContent, $"{review.ContentId}");

            await _reviewRepository.AssignReviewAsync(new Review()
            {
                UserId = userId,
                ContentId = review.ContentId,
                Text = review.Text,
                IsPositive = review.IsPositive,
                Score = -1,
                WrittenAt = DateTimeOffset.UtcNow
            });
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
                ("positive", var reviews) => reviews.Where(r => r.IsPositive),
                ("negative", var reviews) => reviews.Where(r => !r.IsPositive),
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
            else if (review.Score < 0 || review.Score > 10)
            {
                errorMessage = ErrorMessages.ScoreMustBeValid;
                param = nameof(review.Score);
            }

            return errorMessage == null;
        }
    }
}
