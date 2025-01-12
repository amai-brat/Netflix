namespace SupportPermanentS3Service.Services;

public interface ITempToPermanentCopierService
{
    Task CheckCountersAsync(CancellationToken cancellationToken = default);
}