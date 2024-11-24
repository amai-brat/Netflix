using Application.Cqrs.PipelineBehaviors;
using Application.Helpers;
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
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<INotificationService, NotificationService>();

            serviceCollection.AddValidatorsFromAssembly(AssemblyReference.Assembly);
            serviceCollection.AddMediatR(conf =>
            {
                conf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                conf.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            });
            
            return serviceCollection;
        }
    }
}
