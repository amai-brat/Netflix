using Application.Dto;
using Application.Options;
using Application.Services.Abstractions;
using FluentValidation;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Options;
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

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        serviceCollection.AddValidators();
        serviceCollection.AddAutoMapper(typeof(DependencyInjection).Assembly);
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IProfilePicturesProvider, ProfilePicturesProvider>();
        serviceCollection.AddIdentity(configuration);
        serviceCollection.AddOptions(configuration);

        if (environment.IsDevelopment())
        {
            serviceCollection.AddScoped<IEmailSender, FakeEmailSender>();
        }
        else
        {
            serviceCollection.AddScoped<IEmailSender, EmailSender>();
        }

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

        return serviceCollection;
    }
}