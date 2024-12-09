namespace SupportAPI.Models.Abstractions.FileTypes;

public interface IHavingImageFileType: IHavingFiletype
{
    FileType IHavingFiletype.Type
    {
        get => FileType.Image;
        set => Type = value;
    }
}