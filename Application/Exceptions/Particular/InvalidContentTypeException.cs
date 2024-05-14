using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class InvalidContentTypeException(string param, string message = InvalidContentTypeException.DefaultMessage)
    : ArgumentValidationException(message, param)
{
    private const string DefaultMessage = "Такого типа контента быть не должно";
}