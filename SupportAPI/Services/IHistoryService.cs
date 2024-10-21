using SupportAPI.Data.Entities;
using SupportAPI.Models;

namespace SupportAPI.Services
{
    public interface IHistoryService
    {
        Task<SupportChatMessage> SaveMessageAsync(ChatMessageDto chatMessageDto);
        Task<List<SupportChatMessage>> GetMessagesByChatSessionIdAsync(long sessionId);
    }
}
