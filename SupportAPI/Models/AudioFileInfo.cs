using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models;

public class AudioFileInfo: Downloadable
{
    public string? Name { get; set; }
    public string? TimeToListen { get; set; }
    public string? Album { get; set; }
    public string? Author { get; set; }
    public override FileType Type { get; set; } = FileType.Audio;
}