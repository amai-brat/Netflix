namespace Infrastructure.Services.Exceptions;

public class AuthServiceException(string message) : Exception(message);