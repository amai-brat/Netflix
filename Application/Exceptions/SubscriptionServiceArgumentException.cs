namespace Domain.Services.ServiceExceptions;

public class SubscriptionServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName);