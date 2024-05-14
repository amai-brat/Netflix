using Application.Exceptions.Base;

namespace Infrastructure.Identity;

public class IdentityException(string message, string? paramName = null) : ArgumentValidationException(message, paramName);