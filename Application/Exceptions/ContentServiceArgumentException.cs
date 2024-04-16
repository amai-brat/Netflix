namespace Application.Exceptions
{
    public class ContentServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName) { }
}
