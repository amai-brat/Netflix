using SupportPersistentAPI.Data.Entities;

namespace SupportPersistentAPI.Data.Repositories.Interfaces
{
    public interface ISupportChatMessageRepository
    {
        Task<SupportChatMessage> AddChatMessageAsync(SupportChatMessage chatMessage);
        Task<List<SupportChatMessage>> GetChatMessagesByChatSessionIdAsync(long chatSessionId);
    }
}
