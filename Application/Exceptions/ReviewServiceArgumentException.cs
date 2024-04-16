namespace Application.Exceptions
{
    public class ReviewServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName) {}
}
