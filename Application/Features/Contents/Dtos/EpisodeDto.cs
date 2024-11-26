using Microsoft.AspNetCore.Http;

namespace Application.Features.Contents.Dtos;

public class EpisodeDto
{
    public int EpisodeNumber { get; set; }
    public string VideoUrl { get; set; } = null!;
    public int Resolution { get; set; }
    public IFormFile? VideoFile { get; set; }
}