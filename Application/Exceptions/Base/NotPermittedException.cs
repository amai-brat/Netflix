namespace Application.Exceptions.Base;

public class NotPermittedException(string message) : Exception(message);