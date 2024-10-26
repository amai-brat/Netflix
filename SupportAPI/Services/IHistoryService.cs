using SupportAPI.Data.Entities;
using SupportAPI.Models;

namespace SupportAPI.Services
{
    public interface IHistoryService
    {
        Task<ChatMessageEvent> SaveMessageAsync(ChatMessageEvent chatMessageDto);
        Task<List<ChatMessageDto>> GetMessagesByChatSessionIdAsync(long sessionId);
        Task<List<SupportChatSessionDto>> GetUnansweredChatsAsync();
    }
}
