using TagLib;

namespace SupportAPI.Services.MetadataExtractor;

public class BaseMetadataExtractor(string path, string mimeType)
{
    protected readonly TagLib.File File = TagLib.File.Create(path, mimeType, ReadStyle.Average);
    
    public virtual Dictionary<string, string> ExtractMetadata()
    {
        var metadata = new Dictionary<string, string>
        {
            { "name", Path.GetFileName(File.Name) },
            { "mime_type", mimeType },
            { "metadata_extraction_time", DateTime.UtcNow.ToString("s") }
        };

        return metadata;
    }
}