using Microsoft.AspNetCore.Http;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Dto;

public class EpisodeAdminPageDto
{
    public int EpisodeNumber { get; set; }
    public string VideoUrl { get; set; } = null!;
    public int Resolution { get; set; }
    public IFormFile? VideoFile { get; set; }
}