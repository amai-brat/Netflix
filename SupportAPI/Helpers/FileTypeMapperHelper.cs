using Shared.MessageContracts;

namespace SupportAPI.Helpers;

public static class FileTypeMapperHelper
{
    public static FileType MapFileType(string fileType)
    {
        return fileType switch
        {
            "Картинка" => FileType.Image,
            "Аудио" => FileType.Audio,
            "Видео" => FileType.Video,
            "Документ" => FileType.Document,
            "Файл" => FileType.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
    }
}