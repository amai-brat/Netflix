using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public static class ContentAPIServiceRegisterExt
    {
        public static IServiceCollection AddContentService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IContentService, ContentService>();
            return serviceCollection;
        }
        
        public static IServiceCollection AddReviewService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IReviewService, ReviewService>();
            return serviceCollection;
        }
        
        public static IServiceCollection AddFavouriteService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFavouriteService, FavouriteService>();
            
            return serviceCollection;
        }
    }
}
