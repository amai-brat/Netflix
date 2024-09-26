using System.Net;
using Application.Cache;
using Application.Dto;
using Application.Services.Abstractions;
using DataAccess.Cache;
using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Options;
using Infrastructure.Providers.Abstractions;
using Infrastructure.Providers.Implementations;
using Infrastructure.Providers.ProviderFactory;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Implementations;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Minio;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        serviceCollection.AddOptions(configuration)
            .AddValidators()
            .AddAuthProviderResolver()
            .AddRedisCache()
            .AddAutoMapper(typeof(DependencyInjection).Assembly)
            .AddScoped<IUserService, UserService>()
            .AddKeyedSingleton<IMinioClient>(KeyedServices.Avatar, (provider, _) =>
            {
                var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;
            
                return new MinioClient()
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    .WithSSL(options.Secure)
                    .Build();
            })
            .AddKeyedSingleton<IMinioClient>(KeyedServices.Video, (provider, _) =>
            {
                var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;
            
                return new MinioClient()
                    .WithEndpoint(options.ExternalEndpoint, options.Port)
                    .WithCredentials(options.AccessKey, options.SecretKey)
                    .Build();
            })
            .AddScoped<IProfilePicturesProvider, ProfilePicturesProvider>()
            .AddIdentity(configuration);

        if (environment.IsDevelopment())
        {
            serviceCollection.AddScoped<IEmailSender, FakeEmailSender>();
        }
        else
        {
            serviceCollection.AddScoped<IEmailSender, EmailSender>();
        }
        
        serviceCollection.AddScoped<IContentVideoProvider,ContentVideoProvider>();

        return serviceCollection;
    }
    
    private static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<MovieContentAdminPageDto>, MovieContentDtoAdminPageValidator>();
        serviceCollection.AddScoped<IValidator<SerialContentAdminPageDto>, SerialContentDtoAdminPageValidator>();
        serviceCollection.AddScoped<IValidator<SignUpDto>, SignUpDtoValidator>();
        
        return serviceCollection;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<ITokenRepository, TokenRepository>();
        serviceCollection.AddScoped<ITokenGenerator, TokenGenerator>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
        serviceCollection.AddScoped<ITwoFactorTokenSender, TwoFactorTokenSender>();
        
        serviceCollection.AddDbContext<IdentityDbContext>(builder =>
        {
            builder.UseNpgsql(configuration["Database:Identity"]);
        });
        serviceCollection.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequiredLength = 8,
                    RequireNonAlphanumeric = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireDigit = true
                };
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.AllowedUserNameCharacters = 
                    " 0123456789абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            })
            .AddRoles<AppRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<IdentityDbContext>();

        return serviceCollection;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection serviceCollection, 
        IConfiguration configuration)
    {
        serviceCollection.Configure<EmailOptions>(configuration.GetSection("EmailOptions"));
        serviceCollection.Configure<MinioOptions>(configuration.GetSection("Minio"));
        serviceCollection.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        serviceCollection.Configure<GoogleAuthOptions>(configuration.GetSection("Auth:Google"));
        serviceCollection.Configure<VkAuthOptions>(configuration.GetSection("Auth:Vk"));
        serviceCollection.Configure<RedisOptions>(configuration.GetSection("Redis"));
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddAuthProviderResolver(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthProvider, VkAuthProvider>();
        serviceCollection.AddScoped<IAuthProvider, GoogleAuthProvider>();
        serviceCollection.AddScoped<AuthProviderResolver>();
        
        return serviceCollection;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection serviceCollection)
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

        serviceCollection.AddScoped<IDatabase>(sp =>
        {
            var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            
            return multiplexer.GetDatabase();
        });

        serviceCollection.AddScoped<IMinioCache, MinioRedisCache>();
            
        return serviceCollection;
    }
}