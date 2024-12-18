namespace SupportAPI.Options;

public class MinioProxyOptions
{
    public required string Scheme { get; set; }
    public required int Port { get; set; }
    public required string RoutingKey { get; set; }
    public required string Host { get; set; }
}