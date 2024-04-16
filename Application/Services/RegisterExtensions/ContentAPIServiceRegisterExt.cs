using Microsoft.Extensions.DependencyInjection;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Domain.Abstractions;

namespace Application.Services.RegisterExtensions
{
    public static class ContentApiServiceRegisterExt
    {
        public static IServiceCollection AddContentApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IContentService, ContentService>();
            serviceCollection.AddScoped<IReviewService, ReviewService>();
            serviceCollection.AddScoped<IFavouriteService, FavouriteService>();
            serviceCollection.AddScoped<ISubscriptionService, SubscriptionService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<INotificationService, NotificationService>();
            
            return serviceCollection;
        }
    }
}
