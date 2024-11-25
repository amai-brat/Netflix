using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

[Obsolete("CQRS")]
public class UserServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);