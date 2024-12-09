using SupportAPI.Models.Abstractions;
using SupportAPI.Models.Abstractions.BaseFileInfos;

namespace SupportAPI.Models;

public class ImageFileInfo: BaseImageFileInfo, IDownloadableWithFileType
{
    public FileType Type { get; set; } = FileType.Image;
    public required string DownloadUrl { get; set; }
}