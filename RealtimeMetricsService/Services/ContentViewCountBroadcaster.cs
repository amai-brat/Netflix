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

            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
            await channel.ExchangeDeclareAsync(
                exchange: "content-page-views", 
                type: ExchangeType.Direct, 
                cancellationToken: stoppingToken);
            
            var counts = await counter.GetAllViewCountsAsync(stoppingToken);
            foreach (var count in counts)
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(count);
                await channel.BasicPublishAsync(
                    exchange: "content-page-views", 
                    routingKey: count.ContentId.ToString(),
                    mandatory: false, 
                    body: bytes, 
                    cancellationToken: stoppingToken);
            }
            
            await Task.Delay(Timeout, stoppingToken);
        }
    }
}