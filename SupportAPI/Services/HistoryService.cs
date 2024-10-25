﻿using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories.Interfaces;
using SupportAPI.Models;

namespace SupportAPI.Services
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
                throw new Exception("Нет ChatSession с таким ID");
            }

            var chatHistory = await chatMessageRepository
                .GetChatMessagesByChatSessionIdAsync(sessionId);

            var chatMessagesDtos = chatHistory
                .Select(scm =>
                    new ChatMessageDto()
                    {
                        Role = scm.Role,
                        Text = scm.Text
                    }).ToList();

            return chatMessagesDtos;
        }

        public async Task<List<SupportChatSession>> GetUnansweredChatsAsync()
        {
            var res = await chatSessionRepository.GetUserUnansweredChatSessionsAsync();
            res.ForEach(el => el.ChatMessages = null);
            return await chatSessionRepository.GetUserUnansweredChatSessionsAsync();
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
            };

            chatSession.ChatMessages.Add(newChatMessage);

            if (chatMessageEvent.Role == "user")
            {
                chatSession.IsAnswered = false;

                chatSession.UserName ??= chatMessageEvent.SenderName;
            }

            await unitOfWork.SaveChangesAsync();

            return chatMessageEvent;
        }
    }
}
