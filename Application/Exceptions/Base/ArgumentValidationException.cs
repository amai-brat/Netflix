namespace Application.Exceptions.Base;

public class ArgumentValidationException(string message, string? paramName) : ArgumentException(message, paramName);