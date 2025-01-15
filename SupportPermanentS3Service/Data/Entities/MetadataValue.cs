namespace SupportPermanentS3Service.Data.Entities;

// Value
public class MetadataValue
{
    public long Id { get; set; }
    
    public long FileId { get; set; }
    public File? File { get; set; }
    
    public long MetadataId { get; set; }
    public Metadata? Metadata { get; set; }

    public string? Value { get; set; }
}