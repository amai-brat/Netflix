using SupportAPI.Data.Entities;

namespace SupportAPI.Data.Repositories.Interfaces
{
    public interface ISupportChatMessageRepository
    {
        Task<SupportChatMessage> AddChatMessageAsync(SupportChatMessage chatMessage);
        Task<List<SupportChatMessage>> GetChatMessagesByChatSessionIdAsync(long chatSessionId);
    }
}
