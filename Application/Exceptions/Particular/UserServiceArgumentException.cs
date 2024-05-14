using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class UserServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);