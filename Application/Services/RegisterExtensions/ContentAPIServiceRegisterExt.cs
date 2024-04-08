using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Abstractions;
using Application.Services.Implementations;

namespace Application.Services.RegisterExtensions
{
    public static class ContentApiServiceRegisterExt
    {
        public static IServiceCollection AddContentApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IContentService, ContentService>();
            serviceCollection.AddScoped<IReviewService, ReviewService>();
            serviceCollection.AddScoped<IFavouriteService, FavouriteService>();

            return serviceCollection;
        }
    }
}
