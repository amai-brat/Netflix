namespace SupportAPI.Models.Abstractions.BaseFileInfos;

public abstract class BaseAudioFileInfo
{
    public string? Name { get; set; }
    public string? TimeToListen { get; set; }
    public string? Album { get; set; }
    public string? Author { get; set; }
}