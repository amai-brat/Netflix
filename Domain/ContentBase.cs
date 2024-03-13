namespace Domain;

public class ContentBase
{
    public int Id { get; set; }
    public string Name { get; set; }  = null!;
    public string Description { get; set; }  = null!;
    public string? Slogan { get; set; }
    public string PosterUrl { get; set; }  = null!;
    public string VideoUrl { get; set; }  = null!;
    public AgeRating? AgeRating { get; set; }
    public Type Type { get; set; }  = null!;
    public Ratings? Ratings { get; set; }
    public TrailerInfo? TrailerInfo { get; set; }
    public CurrencyValue? Budget { get; set; }
    public List<Genre> Genres { get; set; }  = null!;
    public List<PersonInContent> PersonsInContent { get; set; }  = null!;
    public List<Review>? Reviews { get; set; }
    public List<Subscription> AllowedSubscriptions { get; set; }  = null!;
}