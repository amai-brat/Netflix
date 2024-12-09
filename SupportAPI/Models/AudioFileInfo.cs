using SupportAPI.Models.Abstractions;
using SupportAPI.Models.Abstractions.BaseFileInfos;

namespace SupportAPI.Models;

public class AudioFileInfo: BaseAudioFileInfo, IDownloadableWithFileType
{
    public FileType Type { get; set; } = FileType.Audio;
    public required string DownloadUrl { get; set; }
}