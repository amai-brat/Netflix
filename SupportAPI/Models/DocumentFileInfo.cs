using SupportAPI.Models.Abstractions;

namespace SupportAPI.Models;

public class DocumentFileInfo: Downloadable
{
    public string? Name { get; set; }
    public required DocumentTypes DocType { get; set; }
    public override FileType Type { get; set; } = FileType.Document;
}
public enum DocumentTypes
{
    Pdf,
    Docx,
    Xlsx
}