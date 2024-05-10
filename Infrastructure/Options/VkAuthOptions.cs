namespace Infrastructure.Options;

public class VkAuthOptions
{
    public string AuthUri { get; set; } = null!;
    public string TokenUri { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string InfoReqUri { get; set; } = null!;
}