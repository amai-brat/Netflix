using HotChocolate.Authorization;
using HotChocolate.Language;
using MobileAPI.Exceptions;
using MobileAPI.Models;

namespace MobileAPI.Types.SupportChat;

[ExtendObjectType(OperationType.Query)]
public class MessageQuery
{
    [Authorize]
    [UseProjection]
    public async Task<List<ChatMessageDto>> GetUserSupportChatHistory(
        [Service] ILogger<MessageQuery> logger,
        [Service] IHttpClientFactory clientFactory)
    {
        try
        {
            var client = clientFactory.CreateClient("SupportPersistentApi");
            var response = await client.GetAsync("support/chats/user/messages");
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<List<ChatMessageDto>>();
            return result!;
        }
        catch (Exception e)
        {
            logger.LogWarning("Error during query {Query}: {Message}", nameof(GetUserSupportChatHistory), e.Message);
            throw new ServiceUnavailableException("GetUserSupportChatHistory");
        }
    }
}