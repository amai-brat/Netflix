using SupportAPI.Models.Abstractions;
using SupportAPI.Models.Abstractions.BaseFileInfos;
using SupportAPI.Models.Abstractions.FileTypes;

namespace SupportAPI.Models.Dto.FileInfoDtos;

public class VideoFileInfoDto: BaseAudioFileInfo, IHavingVideoFileType, IHavingFormFile
{
    public required IFormFile File { get; set; }
}