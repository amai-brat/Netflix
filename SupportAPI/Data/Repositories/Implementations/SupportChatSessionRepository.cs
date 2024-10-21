using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories.Interfaces;

namespace SupportAPI.Data.Repositories.Implementations
{
    public class SupportChatSessionRepository(AppDbContext dbContext): ISupportChatSessionRepository
    {
        public async Task<SupportChatSession> CreateAsync(SupportChatSession supportChatSession)
        {
            return (await dbContext.SupportChatSessions
                .AddAsync(supportChatSession)).Entity;
        }

        public async Task<SupportChatSession?> GetChatSessionByIdAsync(long id)
        {
            return await dbContext.SupportChatSessions
                .FirstOrDefaultAsync(scs => scs.Id == id);
        }
    }
}
