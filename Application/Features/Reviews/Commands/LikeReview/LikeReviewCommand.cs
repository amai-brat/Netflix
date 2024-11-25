using Application.Cqrs.Commands;

namespace Application.Features.Reviews.Commands.LikeReview;

public record LikeReviewCommand(long ReviewId, long UserId) : ICommand<LikeReviewDto>;