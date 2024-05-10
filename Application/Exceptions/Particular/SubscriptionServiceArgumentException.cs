using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class SubscriptionServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);