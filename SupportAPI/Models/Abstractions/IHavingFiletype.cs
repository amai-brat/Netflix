namespace SupportAPI.Models.Abstractions;

public interface IHavingFiletype
{
    FileType Type { get; set; } 
}
public enum FileType
{
    Image,
    Audio,
    Video,
    Document,
    Other
}