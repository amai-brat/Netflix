using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class ContentServiceArgumentException(string message, string paramName) 
    : ArgumentValidationException(message, paramName);
