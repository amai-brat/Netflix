using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

[Obsolete("CQRS")]
public class ContentServiceNotPermittedException(string message) : NotPermittedException(message);