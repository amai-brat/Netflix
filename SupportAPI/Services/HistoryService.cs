using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories.Interfaces;
using SupportAPI.Models;

namespace SupportAPI.Services
{
    public class HistoryService(ISupportChatMessageRepository chatMessageRepository,
        ISupportChatSessionRepository chatSessionRepository,
        IUnitOfWork unitOfWork) : IHistoryService
    {
        public async Task<List<SupportChatMessage>> GetMessagesByChatSessionIdAsync(long sessionId)
        {
            var chatHistory = await chatMessageRepository
                .GetChatMessagesByChatSessionIdAsync(sessionId);

            return chatHistory;
        }

        public async Task<SupportChatMessage> SaveMessageAsync(ChatMessageDto chatMessageDto)
        {
            var chatSession = await chatSessionRepository.GetChatSessionByIdAsync(chatMessageDto.ChatSessionId);
            if (chatSession == null)
            {
                var newSupportChatSession = new SupportChatSession()
                {
                    Id = chatMessageDto.ChatSessionId
                };
                chatSession = await chatSessionRepository.CreateAsync(newSupportChatSession);
            }

            var addedChatMessage = await chatMessageRepository
                .AddChatMessageAsync(new SupportChatMessage()
                {
                    ChatSessionId = chatSession.Id,
                    DateTimeSent = chatMessageDto.DateTimeSent,
                    SenderId = chatMessageDto.SenderId,
                    Text = chatMessageDto.Text,
                });

            await unitOfWork.SaveChangesAsync();

            return addedChatMessage;
        }
    }
}
