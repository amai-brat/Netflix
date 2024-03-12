namespace Domain;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CommentText { get; set; }
    public DateTime WrittenAt { get; set; }
    public List<User> LikedByUsers  { get; set; }
}