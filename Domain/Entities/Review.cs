namespace Domain.Entities;

public class Review
{
    public long Id { get; set; }

    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public ContentBase Content { get; set; } = null!;
    public long ContentId { get; set; }

    public string Text { get; set; } = null!;
    public bool IsPositive { get; set; }
    public int Score { get; set; }
    public DateTimeOffset WrittenAt { get; set; }

    public List<Comment>? Comments { get; set; }
    public List<UsersReviews>? RatedByUsers { get; set; }
}