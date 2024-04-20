using Application.Services.Abstractions;
using Infrastructure.Services;
using Infrastucture.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(DependencyInjection).Assembly);
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IProfilePicturesProvider, ProfilePicturesProvider>();
        
        return serviceCollection;
    }
}