using Application.Exceptions.Base;

namespace Application.Identity;

public class IdentityException(string message, string? paramName = null) : ArgumentValidationException(message, paramName);