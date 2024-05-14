using Application.Exceptions.Base;

namespace Application.Exceptions.Particular;

public class ContentServiceNotPermittedException(string message) : NotPermittedException(message);