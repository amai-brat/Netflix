
using Application.Exceptions;
using Domain.Services.ServiceExceptions;
using Infrastructure.Identity;

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
            catch (ArgumentException ex) when (
                ex is ReviewServiceArgumentException ||
                ex is FavouriteServiceArgumentException ||
                ex is ContentServiceArgumentException ||
                ex is CommentServiceArgumentException ||
                ex is NotificationServiceArgumentException ||
                ex is SubscriptionServiceArgumentException ||
                ex is UserServiceArgumentException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = $"{ex.Message}",
                    Code = 400
                });
            }
            catch (IdentityException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = $"{ex.Message}",
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
            catch (BusinessException ex)
            {
                context.Response.StatusCode = 500;
                logger.LogError("Business error happened: {error}", ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = ex.Message,
                    Code = 400
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
