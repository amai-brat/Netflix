using Microsoft.AspNetCore.Mvc;
using Shared;

namespace API.Helper;

public static class ErrorHelper
{
    public static IActionResult Handle(Error error)
    {
        return error.Type switch
        {
            ErrorType.Failure => new StatusCodeResult(500),
            ErrorType.Validation => new BadRequestObjectResult(error.Description),
            ErrorType.NotFound => new NotFoundObjectResult(error.Description),
            ErrorType.Conflict => new ConflictObjectResult(error.Description),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}