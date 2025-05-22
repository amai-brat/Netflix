using Grpc.Core;

namespace SupportAPI.Services.Abstractions;

public interface ISupportChatSessionManager
{
    string CreateUserSession(long userId);
    void BindSessionWithStream(string sessionId, IServerStreamWriter<SupportChatMessage> stream);
    void RemoveUserSession(long userId, string sessionId);
    void JoinUserSessionGroup(long ownerId, string sessionId);
    void LeaveUserSessionGroup(long ownerId, string sessionId);
    bool IsSessionBelongToUser(long userId, string sessionId);
    Task BroadcastMessageToUserGroupAsync(long ownerId, SupportChatMessage message, List<string>? excludeSessionIds = null);
}