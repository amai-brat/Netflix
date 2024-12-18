using Microsoft.Extensions.Options;
using StackExchange.Redis;
using SupportAPI.Options;

namespace SupportAPI.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            ConfigurationOptions configurationOptions = new ConfigurationOptions()
            {
                EndPoints = { { options.Host, options.Port } },
                Password = options.Password,
                AbortOnConnectFail = false
            };
            
            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        return serviceCollection.AddScoped<IDatabase>(sp =>
        {
            var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            
            return multiplexer.GetDatabase();
        });
    }
}