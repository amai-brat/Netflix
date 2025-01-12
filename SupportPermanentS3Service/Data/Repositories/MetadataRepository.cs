using Microsoft.EntityFrameworkCore;
using SupportPermanentS3Service.Data.Entities;
using SupportPermanentS3Service.Repositories;

namespace SupportPermanentS3Service.Data.Repositories;

public class MetadataRepository(AppDbContext dbContext) : IMetadataRepository
{
    public async Task<Metadata> AddMetadataAsync(Metadata metadata)
    {
        var entry = await dbContext.Metadata.AddAsync(metadata);
        return entry.Entity;
    }

    public async Task<Metadata?> GetByNameAsync(string metadataName)
    {
        var metadata = await dbContext.Metadata.FirstOrDefaultAsync(x => x.Name == metadataName);
        return metadata;
    }
}