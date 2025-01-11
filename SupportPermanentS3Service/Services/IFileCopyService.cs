
using SupportPermanentS3Service.Models.Dto;

namespace SupportPermanentS3Service.Services;

public interface IFileCopyService
{
    Task<List<Uri>> CopyFilesAsync(List<Uri> fileUris, CancellationToken cancellationToken);
    Task<List<Uri>> CopyFilesAsync(List<FieldDto> fieldsToCopy, CancellationToken cancellationToken = default);
    public Task GetFileAsync(Stream body, Guid guid, CancellationToken cancellationToken);
    public Task<bool> CanGetFileAsync(Guid guid, string role, int id, CancellationToken cancellationToken);
    public Task<Uri?> GetPresignedUriAsync(Guid guid, CancellationToken cancellationToken);
}