using SupportPermanentS3Service.Data.Entities;

namespace SupportPermanentS3Service.Repositories;

public interface IMetadataValueRepository
{
    Task<MetadataValue> AddMetadataValueAsync(MetadataValue metadataValue);
}