using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

[Obsolete("CQRS")]
public class SubscriptionServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);