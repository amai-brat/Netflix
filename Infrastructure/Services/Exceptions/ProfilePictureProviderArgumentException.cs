namespace Infrastructure.Services.Exceptions;

public class ProfilePictureProviderArgumentException(string message, string paramName) : ArgumentException(message, paramName);