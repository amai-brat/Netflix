namespace Infrastructure.Options;

public class RabbitMqOptions
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Hostname { get; set; }
}