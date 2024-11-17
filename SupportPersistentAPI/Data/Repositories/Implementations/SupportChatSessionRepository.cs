using Microsoft.EntityFrameworkCore;
using SupportPersistentAPI.Data.Repositories.Interfaces;
using SupportPersistentAPI.Data.Entities;

namespace SupportPersistentAPI.Data.Repositories.Implementations
{
    public class SupportChatSessionRepository(AppDbContext dbContext) : ISupportChatSessionRepository
    {
        public async Task<SupportChatSession> CreateAsync(SupportChatSession supportChatSession)
        {
            return (await dbContext.SupportChatSessions
                .AddAsync(supportChatSession)).Entity;
        }

        public async Task<SupportChatSession?> GetChatSessionByIdAsync(long id)
        {
            return await dbContext.SupportChatSessions
                .FindAsync(id);
        }

        public async Task<List<SupportChatSession>> GetUserUnansweredChatSessionsAsync()
        {
            return await dbContext.SupportChatSessions
                .Where(scs => scs.IsAnswered == false)
                .ToListAsync();
        }
    }
}
