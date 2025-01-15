
namespace SupportAPI.Services;

public interface IFileUploadService
{
    public Task<UploadedFileDto> UploadFileAndSaveMetadataAsync(Stream s, string contentType, Dictionary<string,string> metadata, CancellationToken cancellationToken);
    public Task ExtractFileMetadataToTempDatabaseAsync(FieldDto fieldDto, CancellationToken cancellationToken = default);

    public Task DeleteFileAndMetadataAsync(List<string> fileGuids, CancellationToken cancellationToken);
}

public record FieldDto(string BucketName, string ObjectName)
{
    public override string ToString() => $"{BucketName}/{ObjectName}";
}
public record UploadedFileDto(FieldDto Name, string Url);