using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Reviews.Commands.DeleteReview;

internal class DeleteReviewCommandHandler(
    IReviewRepository reviewRepository) : ICommandHandler<DeleteReviewCommand, Review>
{
    public async Task<Review> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetReviewByIdAsync(request.ReviewId);

        if (review == null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundReview, nameof(request.ReviewId));
        }

        var deletedReview = reviewRepository.DeleteReview(review);
        await reviewRepository.SaveChangesAsync();

        return deletedReview;
    }
}