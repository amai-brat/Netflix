using SupportPermanentS3Service.Data.Entities;
using SupportPermanentS3Service.Repositories;

namespace SupportPermanentS3Service.Data.Repositories;

public class MetadataValueRepository(AppDbContext dbContext) : IMetadataValueRepository
{
    public async Task<MetadataValue> AddMetadataValueAsync(MetadataValue metadataValue)
    {
        var entry = await dbContext.Values.AddAsync(metadataValue);
        return entry.Entity;
    }
}