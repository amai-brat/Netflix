using Application.Cqrs.PipelineBehaviors;
using Application.Helpers;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServicesRegisterExt
    {
        public static IServiceCollection AddContentApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPermissionChecker, PermissionChecker>();
            serviceCollection.AddScoped<IContentVideoManager, ContentVideoManager>();

            serviceCollection.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);
            serviceCollection.AddMediatR(conf =>
            {
                conf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                conf.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            });
            
            return serviceCollection;
        }
    }
}
