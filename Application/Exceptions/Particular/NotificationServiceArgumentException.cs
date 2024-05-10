using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class NotificationServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);