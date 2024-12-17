
namespace SupportAPI.Services;

public interface IFileUploadService
{
    public Task<string> UploadFileAndSaveMetadataAsync(Stream s, Dictionary<string,string> metadata, CancellationToken cancellationToken);
    public Task DeleteFileAndMetadataAsync(List<string> fileGuids, CancellationToken cancellationToken);
}