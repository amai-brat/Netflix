using Microsoft.AspNetCore.Http;

namespace Application.Dto;

public class EpisodeAdminPageDto
{
    public int EpisodeNumber { get; set; }
    public string VideoUrl { get; set; } = null!;
    public int Resolution { get; set; }
    public IFormFile? VideoFile { get; set; }
}