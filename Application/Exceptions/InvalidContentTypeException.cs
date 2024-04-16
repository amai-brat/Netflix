namespace Application.Exceptions;

public class InvalidContentTypeException: ArgumentException
{
    private const string DefaultMessage = "Такого типа контента быть не должно";
    public InvalidContentTypeException(string param, string message = DefaultMessage): base(message,param)
    {
        
    }
}