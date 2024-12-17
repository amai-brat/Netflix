using Microsoft.EntityFrameworkCore;
using SupportPersistentAPI.Data.Entities;
using SupportPersistentAPI.Data.Repositories.Interfaces;

namespace SupportPersistentAPI.Data.Repositories.Implementations
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
                .Include(scm => scm.FileInfo)!
                    .ThenInclude(fi => fi.TypeLookup)
                .OrderBy(scm => scm.DateTimeSent)
                .ToListAsync();
        }
    }
}
