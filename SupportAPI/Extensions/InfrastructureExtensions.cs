using Microsoft.Extensions.Options;
using Minio;
using SupportAPI.Options;

namespace SupportAPI.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddOptions(
        this IServiceCollection serviceCollection, 
        IConfiguration configuration)
    {
        serviceCollection.Configure<RedisOptions>(configuration.GetSection("Redis"));
        serviceCollection.Configure<MinioOptions>(configuration.GetSection("Minio"));
        serviceCollection.Configure<MinioOptions>(configuration.GetSection("FileBucketMinioPolicy"));
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddMinio(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMinioClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.Secure)
                .Build();
        });
        
        return serviceCollection;
    }
}