using Application.Exceptions.Base;

namespace Infrastructure.Services.Exceptions;

[Obsolete("CQRS")]
public class AuthServiceException(string message, string? paramName = null) : ArgumentValidationException(message, paramName);