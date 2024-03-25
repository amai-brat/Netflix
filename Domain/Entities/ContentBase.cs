namespace Domain.Entities;

public class ContentBase
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public string? Slogan { get; set; }
	public string PosterUrl { get; set; } = null!;
	public string? Country { get; set; }

	public int ContentTypeId { get; set; }
	public ContentType ContentType { get; set; } = null!;

	public AgeRatings? AgeRatings { get; set; }
	public Ratings? Ratings { get; set; }
	public TrailerInfo? TrailerInfo { get; set; }
	public Budget? Budget { get; set; }

	public List<Genre> Genres { get; set; } = null!;
	public List<PersonInContent> PersonsInContent { get; set; } = null!;
	public List<Review>? Reviews { get; set; }
	public List<Subscription> AllowedSubscriptions { get; set; } = null!;
}