namespace Application.Dto;

[Obsolete("CQRS")]
public class UserReviewDto
{
    public long Id { get; set; }
    public bool IsPositive { get; set; }
    public string Name { get; set; } = null!;
    public int Score { get; set; }
    public string Text { get; set; } = null!;
    public string ContentName { get; set; } = null!;
    public DateTimeOffset WrittenAt { get; set; }
}