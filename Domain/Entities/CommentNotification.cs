namespace Domain.Entities;

public class CommentNotification
{
    public long Id { get; set; }
    
    public bool Readed { get; set; }
    
    public long CommentId { get; set; }
    public Comment Comment { get; set; } = null!;
}