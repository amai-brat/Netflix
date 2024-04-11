
using Application.Exceptions;

namespace API.Middlewares.ExceptionHandler
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ArgumentException ex) when (
            ex is ReviewServiceArgumentException ||
            ex is FavouriteServiceArgumentException ||
            ex is ContentServiceArgumentException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = $"{ex.Message}. {ex.ParamName}",
                    Code = 400
                });
            }
            catch (ContentServiceNotPermittedException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = ex.Message,
                    Code = 403
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = ex.Message + ex.StackTrace,
                    Code = 500
                });
            }
        }
        private class ExceptionDetails
        {
            public string Message { get; set; } = null!;
            public int Code { get; set; }
        }
    }
}
