
namespace SupportPermanentS3Service.Services;

public interface IFileCopyService
{
    Task<List<Uri>> CopyFilesAsync(List<Uri> fileUris, CancellationToken cancellationToken);
    public Task GetFileAsync(Stream body, Guid guid, CancellationToken cancellationToken);
    public Task<bool> CanGetFileAsync(Guid guid, string role, int id, CancellationToken cancellationToken);
}