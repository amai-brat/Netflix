namespace SupportAPI.Services.MetadataExtractor;

public class VideoMetadataExtractor(string path, string mimeType)
    : BaseMetadataExtractor(path, mimeType)
{
    public override Dictionary<string, string> ExtractMetadata()
    {
        var metadata =  base.ExtractMetadata();

        if (File.Properties is not null)
        {
            metadata.Add("video_height", File.Properties.VideoHeight.ToString());
            metadata.Add("video_width", File.Properties.VideoWidth.ToString());
            metadata.Add("video_title", File.Tag.Title);
            metadata.Add("video_duration", File.Properties.Duration.ToString());
        }
        
        return metadata;
    }
}