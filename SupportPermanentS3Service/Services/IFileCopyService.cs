
namespace SupportPermanentS3Service.Services;

public interface IFileCopyService
{
    Task<List<Uri>> CopyFilesAsync(List<Uri> uris, CancellationToken cancellationToken);
}