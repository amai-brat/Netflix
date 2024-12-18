using Shared.MessageContracts;
using SupportPersistentAPI.Data.Entities;
using SupportPersistentAPI.Models;

namespace SupportPersistentAPI.Services
{
    public interface IHistoryService
    {
        Task<ChatMessageEvent> SaveMessageAsync(ChatMessageEvent chatMessageDto);
        Task<List<ChatMessageDto>> GetMessagesByChatSessionIdAsync(long sessionId, string token);
        Task<List<SupportChatSession>> GetUnansweredChatsAsync();
    }
}
