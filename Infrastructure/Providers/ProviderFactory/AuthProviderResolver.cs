using Infrastructure.Providers.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Providers.ProviderFactory;

public class AuthProviderResolver(IServiceProvider serviceProvider)
{
    public IAuthProvider? GetAuthProvider(string provider) =>
        !Enum.TryParse<AuthProvider>(provider, true, out var providerRes) ? 
            null : 
            providerRes switch
            {
                AuthProvider.Vk => serviceProvider.GetServices<IAuthProvider>()
                    .SingleOrDefault(s => s is VkAuthProvider),
                AuthProvider.Google => serviceProvider.GetServices<IAuthProvider>()
                    .SingleOrDefault(s => s is GoogleAuthProvider),
                _ => null
            };
}