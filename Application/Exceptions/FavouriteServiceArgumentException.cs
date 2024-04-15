namespace Application.Exceptions
{
    public class FavouriteServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName) { }
}
