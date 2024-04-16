
namespace Application.Exceptions;

public class CommentServiceArgumentException(string message, string paramName) : ArgumentException(message, paramName);
