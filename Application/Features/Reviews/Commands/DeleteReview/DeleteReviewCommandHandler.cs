using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Reviews.Commands.DeleteReview;

internal class DeleteReviewCommandHandler(
    IReviewRepository reviewRepository,
    IContentRepository contentRepository) : ICommandHandler<DeleteReviewCommand, Review>
{
    public async Task<Review> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetReviewByIdAsync(request.ReviewId);

        if (review == null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundReview, nameof(request.ReviewId));
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
}