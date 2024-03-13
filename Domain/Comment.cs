namespace Domain;

public class Comment
{
    public int Id { get; set; }
    public int ReviewId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime WrittenAt { get; set; }
    public List<User>? ScoredByUsers  { get; set; }
}