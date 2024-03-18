namespace Domain;

public class UsersReviews
{
    public User User { get; set; } = null!;
    public long UserId { get; set; }

    public Review Review { get; set; } = null!;
    public long ReviewId { get; set; }
    
    public bool IsLiked { get; set; }
}