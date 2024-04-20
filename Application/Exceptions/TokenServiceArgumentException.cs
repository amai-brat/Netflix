namespace Application.Exceptions;

public class TokenServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName);