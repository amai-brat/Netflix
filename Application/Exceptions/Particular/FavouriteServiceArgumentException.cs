using Application.Exceptions.Base;

namespace Application.Exceptions.Particular
{
    public class FavouriteServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);
}
