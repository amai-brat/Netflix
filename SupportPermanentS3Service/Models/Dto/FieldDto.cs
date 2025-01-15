namespace SupportPermanentS3Service.Models.Dto;

public record FieldDto(string BucketName, string ObjectName)
{
    public override string ToString() => $"{BucketName}/{ObjectName}";

    public static FieldDto Parse(string field)
    {
        var parts = field.Split('/');
        return new FieldDto(parts[0], parts[1]);
    }
}