using SupportPermanentS3Service.Models.Dto;

namespace SupportPermanentS3Service.Services;

public interface IMetadataCopyService
{
    Task CopyMetadataToDatabaseAsync(List<FieldDto> fieldsToCopy, CancellationToken cancellationToken = default);
}