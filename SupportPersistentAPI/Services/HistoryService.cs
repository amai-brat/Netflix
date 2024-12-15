using SupportPersistentAPI.Data.Repositories.Interfaces;
using SupportPersistentAPI.Data.Entities;
using SupportPersistentAPI.Models;
using Shared.MessageContracts;
using FileInfo = SupportPersistentAPI.Data.Entities.FileInfo;

namespace SupportPersistentAPI.Services
{
    public class HistoryService(ISupportChatMessageRepository chatMessageRepository,
        ISupportChatSessionRepository chatSessionRepository,
        IUnitOfWork unitOfWork) : IHistoryService
    {
        public async Task<List<ChatMessageDto>> GetMessagesByChatSessionIdAsync(long sessionId)
        {
            var chatSession = await chatSessionRepository.GetChatSessionByIdAsync(sessionId);

            if (chatSession == null)
            {
                return [];
            }

            var chatHistory = await chatMessageRepository
                .GetChatMessagesByChatSessionIdAsync(sessionId);

            var chatMessagesDtos = chatHistory
                .Select(scm =>
                    new ChatMessageDto()
                    {
                        Role = scm.Role,
                        Text = scm.Text,
                        Files = scm.FileInfo?.Select(fi =>
                            new FileInfoDto()
                            {
                                Name = fi.Name,
                                Src = fi.Src,
                                Type = fi.TypeLookup.Type
                            }).ToList()
                    }).ToList();

            return chatMessagesDtos;
        }

        public async Task<List<SupportChatSession>> GetUnansweredChatsAsync()
        {
            var res = await chatSessionRepository.GetUserUnansweredChatSessionsAsync();
            res.ForEach(el => el.ChatMessages = null);
            return res;
        }

        public async Task<ChatMessageEvent> SaveMessageAsync(ChatMessageEvent chatMessageEvent)
        {
            var chatSession = await chatSessionRepository.GetChatSessionByIdAsync(chatMessageEvent.ChatSessionId);
            if (chatSession == null)
            {
                var newSupportChatSession = new SupportChatSession()
                {
                    Id = chatMessageEvent.ChatSessionId
                };
                chatSession = await chatSessionRepository.CreateAsync(newSupportChatSession);
            }

            var newChatMessage = new SupportChatMessage()
            {
                DateTimeSent = chatMessageEvent.DateTimeSent,
                SenderId = chatMessageEvent.SenderId,
                SenderName = chatMessageEvent.SenderName,
                Role = chatMessageEvent.Role,
                Text = chatMessageEvent.Text,
                FileInfo = chatMessageEvent.FileInfo?.Select(f => new FileInfo()
                {
                    Name = f.Name,
                    Src = f.Src,
                    Type = f.Type,
                    TypeId = (int)f.Type
                }).ToList()
            };

            chatSession.ChatMessages!.Add(newChatMessage);

            if (chatMessageEvent.Role == "user")
            {
                chatSession.IsAnswered = false;

                chatSession.UserName ??= chatMessageEvent.SenderName;
            }
            else
            {
                chatSession.IsAnswered = true;
            }

            await unitOfWork.SaveChangesAsync();

            return chatMessageEvent;
        }
    }
}
