
namespace SupportAPI.Services;

public interface IFileUploadService
{
    public Task<string> UploadFileAndSaveMetadataAsync(Stream s);
    public Task DeleteFileAndMetadataAsync(List<string> fileGuids);
    public Task<Stream> GetFile(Guid guid);
}