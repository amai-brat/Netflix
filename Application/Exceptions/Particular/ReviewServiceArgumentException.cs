using Application.Exceptions.Base;

namespace Application.Exceptions.Particular
{
    public class ReviewServiceArgumentException(string message, string paramName) 
        : ArgumentValidationException(message, paramName);
}
