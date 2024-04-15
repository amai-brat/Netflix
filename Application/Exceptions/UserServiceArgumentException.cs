namespace Domain.Services.ServiceExceptions;

public class UserServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName);