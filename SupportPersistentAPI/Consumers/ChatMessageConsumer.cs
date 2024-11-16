using MassTransit;
using SupportPersistentAPI.Models;
using SupportPersistentAPI.Services;

namespace SupportPersistentAPI.Consumers
{
    public class ChatMessageConsumer(IHistoryService historyService) : IConsumer<ChatMessageEvent>
    {
        public async Task Consume(ConsumeContext<ChatMessageEvent> context)
        {
            await historyService.SaveMessageAsync(context.Message);
        }
    }
}
