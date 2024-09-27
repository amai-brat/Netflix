namespace Domain.Entities;

public class Comment
{
    public long Id { get; set; }

    public long ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    public long UserId { get; set; }
    public User User { get; set; } = null!;
    
    public long? CommentNotificationId { get; set; }
    public CommentNotification CommentNotification { get; set; } = null!;

    public string Text { get; set; } = null!;
    public DateTimeOffset WrittenAt { get; set; }

    public List<User>? ScoredByUsers  { get; set; }
}