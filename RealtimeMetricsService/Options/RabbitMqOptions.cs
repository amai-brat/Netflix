namespace RealtimeMetricsService.Options;

public class RabbitMqOptions
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string HostUri { get; set; }
    public required string Hostname { get; set; }
}