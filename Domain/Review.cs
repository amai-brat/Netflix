namespace Domain;

public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsPositive { get; set; }
    public string ReviewText { get; set; }
    public DateTime WrittenAt { get; set; }
    public int Score { get; set; }
    public List<User> LikedByUsers  { get; set; }
    public List<Comment> Comments  { get; set; }
}