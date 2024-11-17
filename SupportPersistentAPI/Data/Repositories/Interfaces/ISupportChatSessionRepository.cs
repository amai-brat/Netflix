using SupportPersistentAPI.Data.Entities;

namespace SupportPersistentAPI.Data.Repositories.Interfaces
{
    public interface ISupportChatSessionRepository
    {
        Task<SupportChatSession> CreateAsync(SupportChatSession supportChatSession);
        Task<SupportChatSession?> GetChatSessionByIdAsync(long id);
        Task<List<SupportChatSession>> GetUserUnansweredChatSessionsAsync();
    }
}
