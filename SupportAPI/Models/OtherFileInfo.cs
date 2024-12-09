using SupportAPI.Models.Abstractions;
using SupportAPI.Models.Abstractions.BaseFileInfos;

namespace SupportAPI.Models;

public class OtherFileInfo: BaseOtherFileInfo, IDownloadableWithFileType
{
    public FileType Type { get; set; } = FileType.Other;
    public required string DownloadUrl { get; set; }
}