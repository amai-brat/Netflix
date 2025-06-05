using RealtimeMetricsService;
using RealtimeMetricsService.Helpers;
using RealtimeMetricsService.Options;
using RealtimeMetricsService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransitRabbitMq(
    builder.Configuration.GetSection("RabbitMqOptions").Get<RabbitMqOptions>() 
    ?? throw new InvalidOperationException("RabbitMqOptions not found"));
builder.Services.AddCassandra( 
    builder.Configuration.GetSection("CassandraOptions").Get<CassandraOptions>() 
    ?? throw new InvalidOperationException("CassandraOptions not found"));

builder.Services.AddScoped<IContentViewCounter, CassandraContentViewCounter>();

builder.Services.AddHostedService<ContentViewCountBroadcaster>();

var app = builder.Build();

await Database.CreateViewCounterTableIfNotExists(app.Services);

app.Run();