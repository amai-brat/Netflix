namespace SupportAPI.Models.Abstractions;

public abstract class FileWithType
{
    public virtual FileType Type { get; set; }
}
public enum FileType
{
    Image,
    Audio,
    Video,
    Document,
    Other
}