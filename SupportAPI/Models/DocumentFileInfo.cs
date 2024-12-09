using SupportAPI.Models.Abstractions;
using SupportAPI.Models.Abstractions.BaseFileInfos;

namespace SupportAPI.Models;

public class DocumentFileInfo: BaseDocumentFileInfo ,IDownloadableWithFileType
{
    public FileType Type { get; set; } = FileType.Document;
    public required string DownloadUrl { get; set; }
}