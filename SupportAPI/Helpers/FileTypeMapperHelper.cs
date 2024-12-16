using Shared.MessageContracts;

namespace SupportAPI.Helpers;

public static class FileTypeMapperHelper
{
    public static FileType MapFileType(string fileType)
    {
        return fileType switch
        {
            _ when fileType.Contains("image") => FileType.Image,
            _ when fileType.Contains("audio") => FileType.Audio,
            _ when fileType.Contains("video") => FileType.Video,
            _ => FileType.Document
        };
    }
}