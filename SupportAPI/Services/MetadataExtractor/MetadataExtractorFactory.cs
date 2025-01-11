using System.Net.Mime;

namespace SupportAPI.Services.MetadataExtractor;

public static class MetadataExtractorFactory
{
    private static readonly List<string> ImageMimeTypes = 
    [
        MediaTypeNames.Image.Png,
        MediaTypeNames.Image.Jpeg,
        MediaTypeNames.Image.Svg,
        MediaTypeNames.Image.Gif
    ];

    private static readonly List<string> AudioMimeTypes =
    [
        "audio/aac", // .aac
        "audio/mpeg", // .mp3
        "audio/vnd.wave" // .wav
    ];

    private static readonly List<string> VideoMimeTypes =
    [
        "video/x-matroska", // .mkv
        "video/avi", "video/x-msvideo", // .avi
        "video/mp4", "video/mpeg", // .mp4
        "video/webm" // .webm
    ];

    public static BaseMetadataExtractor Create(string fileMimeType)
    {
        return fileMimeType switch
        {
            _ when ImageMimeTypes.Contains(fileMimeType) => new ImageMetadataExtractor(),
            _ when AudioMimeTypes.Contains(fileMimeType) => new AudioMetadataExtractor(),
            _ when VideoMimeTypes.Contains(fileMimeType) => new VideoMetadataExtractor(),
            _ => new BaseMetadataExtractor()
        };
    }
}