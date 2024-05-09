using Application.Dto;
using Application.Services.Abstractions;
using FluentValidation;
using Infrastructure.Services;
using Infrastructure.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(DependencyInjection).Assembly);
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IProfilePicturesProvider, ProfilePicturesProvider>();
        serviceCollection.AddScoped<IPasswordHasher, PasswordHasher>();
        serviceCollection.AddScoped<IContentVideoProvider,ContentVideoProvider>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<MovieContentAdminPageDto>, MovieContentDtoAdminPageValidator>();
        serviceCollection.AddScoped<IValidator<SerialContentAdminPageDto>, SerialContentDtoAdminPageValidator>();
        serviceCollection.AddScoped<IValidator<SignUpDto>, SignUpDtoValidator>();
        
        return serviceCollection;
    }
}