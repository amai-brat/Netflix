using Domain.Entities;

namespace Application.Dto;

public class SerialContentAdminPageDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Slogan { get; set; }
    public string PosterUrl { get; set; } = null!;
    public string? Country { get; set; }
    public string ContentType { get; set; } = null!;

    public AgeRatings? AgeRating { get; set; }
    public Ratings? Ratings { get; set; }
    public TrailerInfo? TrailerInfo { get; set; }
    public Budget? Budget { get; set; }

    public List<string> Genres { get; set; } = null!;
    public List<PersonInContentAdminPageDto> PersonsInContent { get; set; } = null!;
    public List<SubscriptionAdminPageDto> AllowedSubscriptions { get; set; } = null!;
    
    public List<SeasonInfoAdminPageDto> SeasonInfos { get; set; } = null!;
    public YearRange ReleaseYears { get; set; } = null!;
}