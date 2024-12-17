namespace SupportPermanentS3Service.Options;

public class PermanentMinioOptions
{
    public required string ExternalEndpoint { get; set; }
    public required string Endpoint { get; set; }
    public required int Port { get; set; } 
    public bool Secure { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
}