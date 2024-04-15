
using Domain.Services.ServiceExceptions;

namespace API.Middlewares
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
            ex is ReviewServiceArgumentException or FavouriteServiceArgumentException ||
            ex is ContentServiceArgumentException || 
            ex is SubscriptionServiceArgumentException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"{ex.Message}. {ex.ParamName}");
            }
            catch (ContentServiceNotPermittedException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
