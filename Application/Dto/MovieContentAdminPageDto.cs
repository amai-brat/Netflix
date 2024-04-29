using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Dto;

public class MovieContentAdminPageDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Slogan { get; set; }
    public string PosterUrl { get; set; } = null!;
    public string? Country { get; set; }
    public string ContentType { get; set; } = null!;
    public long MovieLength { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string VideoUrl { get; set; } = null!;
    public IFormFile? VideoFile { get; set; }
    public int Resolution { get; set; }

    public AgeRatings? AgeRatings { get; set; }
    public Ratings? Ratings { get; set; }
    public TrailerInfo? TrailerInfo { get; set; }
    public Budget? Budget { get; set; }
    
    public List<string> Genres { get; set; } = new();
    public List<PersonInContentAdminPageDto> PersonsInContent { get; set; } = new();
    public List<SubscriptionAdminPageDto> AllowedSubscriptions { get; set; } = new();
}