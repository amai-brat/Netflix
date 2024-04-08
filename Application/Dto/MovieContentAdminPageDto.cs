using Domain.Entities;

namespace Application.Dto;

public class MovieContentAdminPageDto
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Slogan { get; set; }
    public string Poster { get; set; } = null!;
    public string? Country { get; set; }
    public string ContentType { get; set; } = null!;
    public long MovieLength { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string VideoUrl { get; set; } = null!;
    
    public AgeRatingAdminPageDto? AgeRating { get; set; }
    public Ratings? Ratings { get; set; }
    public TrailerInfo? TrailerInfo { get; set; }
    public Budget? Budget { get; set; }
    
    public List<string> Genres { get; set; } = null!;
    public List<PersonInContentAdminPageDto> PersonsInContent { get; set; } = null!;
    public List<SubscriptionAdminPageDto> AllowedSubscriptions { get; set; } = null!;
}