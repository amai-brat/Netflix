using SupportPermanentS3Service.Repositories;
using File = SupportPermanentS3Service.Data.Entities.File;

namespace SupportPermanentS3Service.Data.Repositories;

public class FileRepository(AppDbContext dbContext) : IFileRepository
{
    public async Task<File> AddFileAsync(File file)
    {
        var entry = await dbContext.Files.AddAsync(file);
        return entry.Entity;
    }
}