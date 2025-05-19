namespace MobileAPI.Types.Subscriptions;

public class SubscriptionServiceErrorDto
{
    public required string Message { get; set; }
    public string Error { get; set; } = null!;
    public int StatusCode { get; set; }
}