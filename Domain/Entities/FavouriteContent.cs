namespace Domain;

public class FavouriteContent
{
    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public ContentBase Content { get; set; } = null!;
    public long ContentId { get; set; }

    public DateTimeOffset AddedAt { get; set; }
}