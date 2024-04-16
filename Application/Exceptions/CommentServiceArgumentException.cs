namespace Domain.Services.ServiceExceptions;

public class CommentServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName) { }