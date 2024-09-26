namespace API.Middlewares.ExceptionHandler
{
    public static class ExceptionHandlerMiddlewareExt
    {
        public static IServiceCollection AddExceptionHandlerMiddleware(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<ExceptionHandlerMiddleware>();
        }
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
