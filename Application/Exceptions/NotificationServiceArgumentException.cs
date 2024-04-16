namespace Domain.Services.ServiceExceptions;

public class NotificationServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName) { }