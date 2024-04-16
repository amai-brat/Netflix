using Application.Dto;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.RegisterExtensions;

public static class ValidatorRegisterExt
{
    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<MovieContentAdminPageDto>, MovieContentDtoAdminPageValidator>();
        serviceCollection.AddScoped<IValidator<SerialContentAdminPageDto>, SerialContentDtoAdminPageValidator>();
        serviceCollection.AddScoped<IValidator<SignUpDto>, SignUpDtoValidator>();
        
        return serviceCollection;
    }
}