using Application.Dto;
using Application.Services.Abstractions;
using FluentValidation;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Implementations;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddValidators();
        serviceCollection.AddAutoMapper(typeof(DependencyInjection).Assembly);
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IProfilePicturesProvider, ProfilePicturesProvider>();
        serviceCollection.AddIdentity(configuration);

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
            .AddEntityFrameworkStores<IdentityDbContext>();

        return serviceCollection;
    }
}