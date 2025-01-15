namespace SupportAPI.Services.MetadataExtractor;

public class ImageMetadataExtractor(string path, string mimeType)
    : BaseMetadataExtractor(path, mimeType)
{
    public override Dictionary<string, string> ExtractMetadata()
    {
        var metadata =  base.ExtractMetadata();
        
        if (File.Properties is not null)
        {
            metadata.Add("image_height", File.Properties.PhotoHeight.ToString());
            metadata.Add("image_width", File.Properties.PhotoWidth.ToString());
            metadata.Add("image_quality", File.Properties.PhotoQuality.ToString());
        }
        
        return metadata;
    }
}