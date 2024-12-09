namespace SupportAPI.Models.Abstractions.BaseFileInfos;

public abstract class BaseVideoFileInfo
{
    public string? Name { get; set; }
    // поддержка должна отмотать видео до этого времени
    public string? TimeToWatch { get; set; }
    public string? Studio { get; set; }
}