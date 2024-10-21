using SupportAPI.Data.Entities;

namespace SupportAPI.Data.Repositories.Interfaces
{
    public interface ISupportChatSessionRepository
    {
        Task<SupportChatSession> CreateAsync(SupportChatSession supportChatSession);
        Task<SupportChatSession?> GetChatSessionByIdAsync(long id);
    }
}
