using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using StackExchange.Redis;
using SupportPermanentS3Service.Enums;
using SupportPermanentS3Service.Options;

namespace SupportPermanentS3Service.Extensions;

public static class InfrastructureExtensions
{
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
        });

        return serviceCollection;
    }
    
    public static IServiceCollection AddOptions(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<PermanentMinioOptions>(configuration.GetSection("PermMinio"));
        serviceCollection.Configure<TemporaryMinioOptions>(configuration.GetSection("TempMinio"));
        serviceCollection.Configure<RedisOptions>(configuration.GetSection("Redis"));
        serviceCollection.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqConfig"));
        serviceCollection.Configure<MinioProxyOptions>(configuration.GetSection("ProxyMinio"));
        return serviceCollection;
    }

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
    
    public static IServiceCollection AddMinios(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddKeyedSingleton<IMinioClient>(KeyedMinios.Permanent, (sp,_) =>
        {
            var options = sp.GetRequiredService<IOptions<PermanentMinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.ExternalEndpoint, options.Port)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.Secure)
                .Build();
        });
        
        serviceCollection.AddKeyedSingleton<IMinioClient>(KeyedMinios.Temporary, (sp,_) =>
        {
            var options = sp.GetRequiredService<IOptions<TemporaryMinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.ExternalEndpoint, options.Port)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.Secure)
                .Build();
        });
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hub/support"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return serviceCollection;
    }
}