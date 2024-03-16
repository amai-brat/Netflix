namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public DateOnly? BirthDay { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<Review>? Reviews { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Comment>? ScoredComments { get; set; }
    public List<FavouriteContent>? FavouriteContent { get; set; }
}