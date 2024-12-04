using Application.Cqrs.Commands;

namespace Application.Features.Reviews.Commands.AssignReview;

public record AssignReviewCommand(ReviewAssignDto AssignDto, long UserId) : ICommand;

public class ReviewAssignDto
{
    public long ContentId { get; set; }
    public string Text { get; set; } = null!;
    public bool IsPositive { get; set; }
    public int? Score { get; set; }
}