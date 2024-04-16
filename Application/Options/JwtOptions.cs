namespace Application.Options;

public class JwtOptions
{
    public string Key { get; set; } = null!;
    public int AccessTokenLifetimeInMinutes { get; set; }
    public int RefreshTokenLifetimeInDays { get; set; }
}