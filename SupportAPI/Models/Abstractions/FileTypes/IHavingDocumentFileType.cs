namespace SupportAPI.Models.Abstractions.FileTypes;

public interface IHavingDocumentFileType: IHavingFiletype
{
    FileType IHavingFiletype.Type
    {
        get => FileType.Document;
        set => Type = value;
    }
}