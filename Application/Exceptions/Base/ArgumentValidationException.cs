namespace Application.Exceptions.Base;

public class ArgumentValidationException(string message, string? paramName = null) : ArgumentException(message, paramName);