using MassTransit;
using SupportAPI.Models;
using SupportAPI.Services;

namespace SupportAPI.Consumers
{
    public class ChatMessageConsumer(IHistoryService historyService): IConsumer<ChatMessageDto>
    {
        public async Task Consume(ConsumeContext<ChatMessageDto> context)
        {
            await historyService.SaveMessageAsync(context.Message);
        }
    }
}
