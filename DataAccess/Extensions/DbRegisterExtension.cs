﻿using Application.Repositories;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IReviewRepository = Application.Repositories.IReviewRepository;

namespace DataAccess.Extensions
{
    public static class DbRegisterExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
        {
            serviceCollection.AddScoped<IReviewRepository, ReviewRepository>();
            serviceCollection.AddScoped<IContentRepository, ContentRepository>();
            serviceCollection.AddScoped<IFavouriteContentRepository, FavouriteContentRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<ICommentRepository, CommentRepository>();
            serviceCollection.AddScoped<ICommentNotificationRepository, CommentNotificationRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            serviceCollection.AddScoped<IContentTypeRepository, ContentTypeRepository>();
            serviceCollection.AddScoped<IGenreRepository, GenreRepository>();
            
            return serviceCollection.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseNpgsql(configuration["Database:ConnectionString"]);
                builder.UseSnakeCaseNamingConvention();
            });
        }
    }
}
