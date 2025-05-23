using Grpc.Core;

namespace SupportAPI.Services.Abstractions;

public interface IChatSessionManager<T> where T : class
{
    string CreateUserSession(long userId);
    void BindSessionWithStream(string sessionId, IServerStreamWriter<T>? stream);
    void RemoveUserSession(long userId, string sessionId);
    void JoinUserSessionGroup(long ownerId, string sessionId);
    void LeaveUserSessionGroup(long ownerId, string sessionId);
    bool IsSessionBelongToUser(long userId, string sessionId);
    Task BroadcastMessageToUserGroupAsync(long ownerId, T message, params string[] excludeSessionIds);
    Task NotifySessions(T message, params string[] sessionIds);
}