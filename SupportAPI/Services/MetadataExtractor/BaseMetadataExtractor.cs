using TagLib;

namespace SupportAPI.Services.MetadataExtractor;

public class BaseMetadataExtractor
{
    public virtual Dictionary<string, string> ExtractMetadata(string path, string mimeType)
    {
        var metadata = new Dictionary<string, string>();
        var file = TagLib.File.Create(path, mimeType, ReadStyle.None);
        
        metadata.Add("name", Path.GetFileName(file.Name));
        metadata.Add("metadata_extraction_time", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

        return metadata;
    }
}