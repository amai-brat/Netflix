namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime? BirthDay { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Review>? Reviews { get; set; }
    public List<Review>? LikedReviews { get; set; }
    public List<Comment>? LikedComments { get; set; }
    public List<FavouriteContent>? FavouriteContent { get; set; }
}