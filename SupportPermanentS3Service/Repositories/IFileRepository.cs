using File = SupportPermanentS3Service.Data.Entities.File;

namespace SupportPermanentS3Service.Repositories;

public interface IFileRepository
{
    Task<File> AddFileAsync(File file);
}