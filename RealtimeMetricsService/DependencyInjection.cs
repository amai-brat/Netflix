using Cassandra;
using MassTransit;
using RabbitMQ.Client;
using RealtimeMetricsService.Consumers;
using RealtimeMetricsService.Options;

namespace RealtimeMetricsService;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransitRabbitMq(
        this IServiceCollection services,
        RabbitMqOptions rabbitMqOptions)
    {
        services.AddSingleton<IConnection>(_ => new ConnectionFactory
        {
            UserName = rabbitMqOptions.Username,
            Password = rabbitMqOptions.Password,
            HostName = rabbitMqOptions.Hostname,
        }.CreateConnection());
        
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<ContentPageOpenedConsumer>();
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(rabbitMqOptions.HostUri), h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection AddCassandra(
        this IServiceCollection services,
        CassandraOptions cassandraOptions)
    {
        services.AddSingleton<Cluster>(_ => Cluster.Builder()
            .AddContactPoint(cassandraOptions.Hostname)
            .WithCredentials(cassandraOptions.Username, cassandraOptions.Password)
            .Build());
        
        return services;
    }
}