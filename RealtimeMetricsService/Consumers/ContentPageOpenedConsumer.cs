using System.Text.Json;
using MassTransit;
using RabbitMQ.Client;
using RealtimeMetricsService.Models;
using RealtimeMetricsService.Services;
using Shared.MessageContracts;

namespace RealtimeMetricsService.Consumers;

public class ContentPageOpenedConsumer(
    ILogger<ContentPageOpenedConsumer> logger,
    IConnection connection,
    IContentViewCounter counter) : IConsumer<ContentPageOpenedEvent>
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    
    public async Task Consume(ConsumeContext<ContentPageOpenedEvent> context)
    {
        logger.LogInformation("Consuming ContentPageOpenedEvent of content {ContentId}", context.Message.ContentId);   
        await counter.AddViewsAsync(context.Message.ContentId, 1, context.CancellationToken);
        await BroadcastViewsCount(context.Message.ContentId, context.CancellationToken);
    }

    private async Task BroadcastViewsCount(long contentId, CancellationToken cancellationToken = default)
    {
        var views = await counter.GetViewCountAsync(contentId, cancellationToken);
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(
            exchange: "content-page-views",
            type: ExchangeType.Direct);
        
        var bytes = JsonSerializer.SerializeToUtf8Bytes(new ContentViewCount
        {
            ContentId = contentId,
            Views = views
        }, _serializerOptions);
        channel.BasicPublish(
            exchange: "content-page-views",
            routingKey: contentId.ToString(),
            mandatory: false,
            body: bytes);
    }
}