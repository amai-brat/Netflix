using Infrastructure.Providers.Implementations;
using Infrastructure.Providers.ProviderFactory;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Providers.Extensions;

public static class AddAuthProviderResolverExt
{
    public static IServiceCollection AddAuthProviderResolver(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthProvider, VkAuthProvider>();
        serviceCollection.AddScoped<IAuthProvider, GoogleAuthProvider>();
        serviceCollection.AddScoped<AuthProviderResolver>();
        
        return serviceCollection;
    }
}