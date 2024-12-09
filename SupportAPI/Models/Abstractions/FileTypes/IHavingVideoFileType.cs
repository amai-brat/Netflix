namespace SupportAPI.Models.Abstractions.FileTypes;

public interface IHavingVideoFileType: IHavingFiletype
{
    FileType IHavingFiletype.Type
    {
        get => FileType.Video;
        set => Type = value;
    }
}