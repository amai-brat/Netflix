using Application.Cqrs.PipelineBehaviors;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServicesRegisterExt
    {
        public static IServiceCollection AddContentApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IContentService, ContentService>();
            serviceCollection.AddScoped<IReviewService, ReviewService>();
            serviceCollection.AddScoped<IFavouriteService, FavouriteService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<ISubscriptionService, SubscriptionService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<INotificationService, NotificationService>();

            serviceCollection.AddValidatorsFromAssembly(typeof(ServicesRegisterExt).Assembly);
            serviceCollection.AddMediatR(conf =>
            {
                conf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                conf.RegisterServicesFromAssembly(typeof(ServicesRegisterExt).Assembly);
            });
            
            return serviceCollection;
        }
    }
}
