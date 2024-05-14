namespace Infrastructure.Options;

public class MinioOptions
{
    public string ExternalEndpoint { get; set; } = null!;
    public string Endpoint { get; set; } = null!;
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}