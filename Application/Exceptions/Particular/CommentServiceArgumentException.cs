
using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class CommentServiceArgumentException(string message, string paramName) : ArgumentValidationException(message, paramName);
