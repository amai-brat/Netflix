using System.Net.Http.Headers;
using SupportPersistentAPI.Data.Repositories.Interfaces;
using SupportPersistentAPI.Data.Entities;
using SupportPersistentAPI.Models;
using Shared.MessageContracts;
using FileInfo = SupportPersistentAPI.Data.Entities.FileInfo;

namespace SupportPersistentAPI.Services
{
    public class HistoryService(ISupportChatMessageRepository chatMessageRepository,
        ISupportChatSessionRepository chatSessionRepository,
        IHttpClientFactory factory,
        IUnitOfWork unitOfWork) : IHistoryService
    {
        public async Task<List<ChatMessageDto>> GetMessagesByChatSessionIdAsync(long sessionId,string token)
        {
            var chatSession = await chatSessionRepository.GetChatSessionByIdAsync(sessionId);

            if (chatSession == null)
            {
                return [];
            }

            var chatHistory = await chatMessageRepository
                .GetChatMessagesByChatSessionIdAsync(sessionId);
            Dictionary<long, Uri> presignedUris = new();
            foreach (var scm in chatHistory)
            {
                if (scm.FileInfo != null)
                {
                    foreach (var fileInfo in scm.FileInfo)
                    {
                        presignedUris.Add(scm.Id, await GetPresignedUriAsync(Guid.Parse(fileInfo.Src), token));
                    }
                }
            }
            
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
                                Src = presignedUris[scm.Id],
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

        private async Task<Uri> GetPresignedUriAsync(Guid guid, string token, CancellationToken cancellationToken = new())
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                token);
            var resp = await client.GetAsync(
                "http://support-permanent-s3-service:8080/get-file-uris" + $"?guid={guid.ToString()}", cancellationToken);

            return (await resp.Content.ReadFromJsonAsync<Uri>(cancellationToken))!;
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
                    Src = f.Src.Segments[^1].Replace("/",""),
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
