using System.Net;
using Application.Dto;
using Application.Services.Abstractions;
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

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        serviceCollection.AddOptions(configuration);
        serviceCollection.AddValidators();
        serviceCollection.AddAuthProviderResolver();
        serviceCollection.AddAutoMapper(typeof(DependencyInjection).Assembly);
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddKeyedSingleton<IMinioClient>(KeyedServices.Avatar,(provider,_) =>
        {
            var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;
            
            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL()
                .Build();
        });
        serviceCollection.AddKeyedSingleton<IMinioClient>(KeyedServices.Video,(provider,_) =>
        {
            var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;
            
            return new MinioClient()
                .WithEndpoint(options.ExternalEndpoint, options.Port)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .Build();
        });
        serviceCollection.AddScoped<IProfilePicturesProvider, ProfilePicturesProvider>();
        serviceCollection.AddIdentity(configuration);

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

        return serviceCollection;
    }
    
    private static IServiceCollection AddAuthProviderResolver(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthProvider, VkAuthProvider>();
        serviceCollection.AddScoped<IAuthProvider, GoogleAuthProvider>();
        serviceCollection.AddScoped<AuthProviderResolver>();
        
        return serviceCollection;
    }
    
    
}