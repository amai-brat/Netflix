using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
        serviceCollection.Configure<FilePathOptions>(configuration.GetSection("FilePath"));
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddMinio(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMinioClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.ExternalEndpoint, options.Port)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.Secure)
                .Build();
        });
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddSwaggerGenWithBearer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Description = """
                              Authorization using JWT by adding header
                              Authorization: Bearer [token]
                              """,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    Array.Empty<string>()
                }
            });
            options.AddServer(new OpenApiServer()
            {
                Url = "",
                Description = "Base path for Support Hub"
            });
        });
        
        
        return serviceCollection;
    }
    
}