using Domain.Entities;

namespace Application.Features.Contents.Dtos;

public class SerialContentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Slogan { get; set; }
    public string PosterUrl { get; set; } = null!;
    public string BigPosterUrl { get; set; } = null!;
    public string? Country { get; set; }
    public string ContentType { get; set; } = null!;

    public AgeRatings? AgeRating { get; set; }
    public Ratings? Ratings { get; set; }
    public TrailerInfo? TrailerInfo { get; set; }
    public Budget? Budget { get; set; }

    public List<string> Genres { get; set; } = new();
    public List<PersonInContentDto> PersonsInContent { get; set; } = new();
    public List<SubscriptionDto> AllowedSubscriptions { get; set; } = new();
    
    public List<SeasonInfoDto> SeasonInfos { get; set; } = new();
    public YearRange ReleaseYears { get; set; } = null!;
}