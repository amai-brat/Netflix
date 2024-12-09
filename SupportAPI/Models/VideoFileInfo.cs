using SupportAPI.Models.Abstractions;
using SupportAPI.Models.Abstractions.BaseFileInfos;

namespace SupportAPI.Models;

public class VideoFileInfo: BaseVideoFileInfo, IDownloadableWithFileType
{
    public FileType Type { get; set; } = FileType.Video;
    public required string DownloadUrl { get; set; }
}