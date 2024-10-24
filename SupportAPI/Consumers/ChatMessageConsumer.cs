using MassTransit;
using SupportAPI.Models;
using SupportAPI.Services;

namespace SupportAPI.Consumers
{
    public class ChatMessageConsumer(IHistoryService historyService): IConsumer<ChatMessageEvent>
    {
        public async Task Consume(ConsumeContext<ChatMessageEvent> context)
        {
            await historyService.SaveMessageAsync(context.Message);
        }
    }
}
