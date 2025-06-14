namespace RealtimeMetricsService.Options;

public class CassandraOptions
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Hostname { get; set; }
}