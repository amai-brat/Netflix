using Application.Cqrs.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Reviews.Commands.AssignReview;

internal class AssignReviewCommandHandler(
    IReviewRepository reviewRepository,
    IContentRepository contentRepository) : ICommandHandler<AssignReviewCommand>
{
    public async Task Handle(AssignReviewCommand request, CancellationToken cancellationToken)
    {
        await reviewRepository.AssignReviewAsync(new Review
        {
            UserId = request.UserId,
            ContentId = request.AssignDto.ContentId,
            Text = request.AssignDto.Text,
            IsPositive = request.AssignDto.IsPositive,
            Score = request.AssignDto.Score ?? -1,
            WrittenAt = DateTimeOffset.UtcNow
        });
            
        var content = await contentRepository.GetContentByFilterAsync(c => c.Id == request.AssignDto.ContentId);
        var reviewCount = await reviewRepository.GetReviewsCountAsync(request.AssignDto.ContentId);
        if (content!.Ratings == null)
        {
            content.Ratings = new Ratings();
        }
        content
                .Ratings
                .LocalRating =
            ((content.Ratings.LocalRating ?? 0) * reviewCount + request.AssignDto.Score!.Value)
            / (reviewCount);
            
        // format float local rating to 2 decimal places
        content.Ratings.LocalRating = (float) Math.Round(content.Ratings.LocalRating.Value, 2);

        await contentRepository.SaveChangesAsync();
    }
}