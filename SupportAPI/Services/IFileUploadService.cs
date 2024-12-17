
namespace SupportAPI.Services;

public interface IFileUploadService
{
    public Task<string> UploadFileAndSaveMetadataAsync(Stream s, Dictionary<string,string> metadata, CancellationToken cancellationToken);
    public Task DeleteFileAndMetadataAsync(List<string> fileGuids, CancellationToken cancellationToken);
    public Task<Stream?> GetFileAsync(Guid guid, CancellationToken cancellationToken);
    public Task<bool> CanGetFileAsync(Guid guid, string role, int id, CancellationToken cancellationToken);
}