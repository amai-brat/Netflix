namespace SupportAPI.Models.Abstractions.BaseFileInfos;

public abstract class BaseDocumentFileInfo
{
    public string? Name { get; set; }
    public required DocumentTypes DocType { get; set; }
}
public enum DocumentTypes
{
    Pdf,
    Docx,
    Xlsx
}