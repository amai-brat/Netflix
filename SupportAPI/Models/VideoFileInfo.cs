using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models;

public class VideoFileInfo: Downloadable
{
    public string? Name { get; set; }
    // поддержка должна отмотать видео до этого времени
    public string? TimeToWatch { get; set; }
    public string? Studio { get; set; }
    public override FileType Type { get; set; } = FileType.Video;
}