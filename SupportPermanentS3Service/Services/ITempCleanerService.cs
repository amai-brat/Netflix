namespace SupportPermanentS3Service.Services;

public interface ITempCleanerService
{
    Task CleanTempAsync(CancellationToken cancellationToken = default);
}