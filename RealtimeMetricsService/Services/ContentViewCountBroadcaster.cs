using System.Text.Json;
using RabbitMQ.Client;

namespace RealtimeMetricsService.Services;

public class ContentViewCountBroadcaster(
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var counter = scope.ServiceProvider.GetRequiredService<IContentViewCounter>();
            var connection = scope.ServiceProvider.GetRequiredService<IConnection>();

            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(
                exchange: "content-page-views",
                type: ExchangeType.Direct);

            var counts = await counter.GetAllViewCountsAsync(stoppingToken);
            foreach (var count in counts)
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(count, new JsonSerializerOptions {
                  PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
                channel.BasicPublish(
                    exchange: "content-page-views",
                    routingKey: count.ContentId.ToString(),
                    mandatory: false,
                    body: bytes);
            }

            await Task.Delay(Timeout, stoppingToken);
        }
    }
}
