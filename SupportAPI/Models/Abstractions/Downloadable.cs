namespace SupportAPI.Models.Abstractions;

public abstract class Downloadable: FileWithType
{
    public required string DownloadUrl { get; set; }
}
public enum Type
{
    Image,
    Audio,
    Video,
    Document,
    Other
}