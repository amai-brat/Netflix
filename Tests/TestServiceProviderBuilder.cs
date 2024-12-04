using Application.Cqrs.PipelineBehaviors;
using Application.Helpers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class TestServiceProviderBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection()
        .AddMediatR(conf =>
        {
            conf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            conf.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        })
        .AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

    public TestServiceProviderBuilder With(Action<IServiceCollection> register)
    {
        register(_services);
        return this;
    }

    public IServiceProvider Build()
    {
        return _services.BuildServiceProvider();
    }
}