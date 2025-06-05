using MassTransit;
using RealtimeMetricsService.Services;
using Shared.MessageContracts;

namespace RealtimeMetricsService.Consumers;

public class ContentPageOpenedConsumer(
    ILogger<ContentPageOpenedConsumer> logger,
    IContentViewCounter counter) : IConsumer<ContentPageOpenedEvent>
{
    public async Task Consume(ConsumeContext<ContentPageOpenedEvent> context)
    {
        logger.LogInformation("Consuming ContentPageOpenedEvent of content {ContentId}", context.Message.ContentId);   
        await counter.AddViewsAsync(context.Message.ContentId, 1);
    }
}