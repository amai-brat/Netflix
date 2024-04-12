using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Services;

public static class NotifyAPIServiceRegisterExt
{
    public static IServiceCollection AddCommentService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICommentService, CommentService>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddNotificationService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INotificationService, NotificationService>();
        return serviceCollection;
    }
}