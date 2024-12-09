namespace SupportAPI.Models.Abstractions.FileTypes;

public interface IHavingOtherFileType: IHavingFiletype
{
    FileType IHavingFiletype.Type
    {
        get => FileType.Other;
        set => Type = value;
    }
}