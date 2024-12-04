using Application.Cqrs.Commands;
using Application.Repositories;

namespace Application.Features.Reviews.Commands.LikeReview;

internal class LikeReviewCommandHandler(
    IReviewRepository reviewRepository) : ICommandHandler<LikeReviewCommand, LikeReviewDto>
{
    public async Task<LikeReviewDto> Handle(LikeReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await reviewRepository.IsReviewLikedByUserAsync(request.ReviewId, request.UserId))
        {
            await reviewRepository.AddReviewLikeAsync(request.ReviewId, request.UserId);
            await reviewRepository.SaveChangesAsync();
            return new LikeReviewDto { IsSuccessful = true };
        }

        await reviewRepository.RemoveReviewLikeAsync(request.ReviewId, request.UserId);
        await reviewRepository.SaveChangesAsync();

        return new LikeReviewDto { IsSuccessful = false };
    }
}