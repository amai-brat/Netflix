namespace SupportAPI.Services.MetadataExtractor;

public class AudioMetadataExtractor(string path, string mimeType) 
    : BaseMetadataExtractor(path, mimeType)
{
    public override Dictionary<string, string> ExtractMetadata()
    {
        var metadata =  base.ExtractMetadata();
        
        metadata.Add("album", File.Tag.Album);
        metadata.Add("artist", string.Join(", ", File.Tag.Performers));
        metadata.Add("audio_title", File.Tag.Title);
        metadata.Add("audio_duration", File.Properties.Duration.ToString());
        
        return metadata;
    }
}