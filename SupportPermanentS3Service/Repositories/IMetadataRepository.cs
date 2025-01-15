using SupportPermanentS3Service.Data.Entities;

namespace SupportPermanentS3Service.Repositories;

public interface IMetadataRepository
{
    Task<Metadata> AddMetadataAsync(Metadata metadata);
    Task<Metadata?> GetByNameAsync(string metadataName);
}