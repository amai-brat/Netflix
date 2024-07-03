using Application.Exceptions.Base;

namespace API.Middlewares.ExceptionHandler
{
    public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ArgumentValidationException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = $"{ex.Message}",
                    Code = 400
                });
                
                logger.LogWarning(ex.Message + ex.StackTrace);
            }
            catch (NotPermittedException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = ex.Message,
                    Code = 403
                });
                
                logger.LogWarning(ex.Message + ex.StackTrace);
            }
            catch (BusinessException ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = "Internal server error",
                    Code = 500
                });
                
                logger.LogWarning("Business error happened: {error}", ex.Message + ex.StackTrace);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = "Internal server error",
                    Code = 500
                });
                
                logger.LogWarning("Unhandled exception: {error}", ex.Message + ex.StackTrace);
            }
        }
        private class ExceptionDetails
        {
            public string Message { get; set; } = null!;
            public int Code { get; set; }
        }
    }
}
