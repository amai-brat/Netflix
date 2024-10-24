using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories.Interfaces;

namespace SupportAPI.Data.Repositories.Implementations
{
    public class SupportChatMessageRepository(AppDbContext dbContext) : ISupportChatMessageRepository
    {
        public async Task<SupportChatMessage> AddChatMessageAsync(SupportChatMessage chatMessage)
        {
            return (await dbContext.SupportChatMessages
                .AddAsync(chatMessage)).Entity;
        }

        public async Task<List<SupportChatMessage>> GetChatMessagesByChatSessionIdAsync(long chatSessionId)
        {
            return await dbContext.SupportChatMessages
                .Where(scm => scm.ChatSessionId == chatSessionId)
                .OrderBy(scm => scm.DateTimeSent)
                .ToListAsync();
        }
    }
}
