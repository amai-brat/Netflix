using System.Collections.Concurrent;
using SupportAPI.Services.Abstractions;

namespace SupportAPI.Services.Implementations;

public class SupportChatSessionManager: ChatSessionManager<SupportChatMessage>, ISupportChatSessionManager<SupportChatMessage>
{
    private readonly Dictionary<SupportChatRole, ConcurrentDictionary<string, object?>> _roleSessions = new()
    {
        { SupportChatRole.Support, [] },
        { SupportChatRole.User, [] }
    };

    public override string CreateUserSession(long userId)
        => CreateUserSession(userId, SupportChatRole.User);

    public override void RemoveUserSession(long userId, string sessionId) 
        => RemoveUserSession(userId, sessionId, SupportChatRole.User);

    public override Task BroadcastMessageToUserGroupAsync(long ownerId, SupportChatMessage message, params string[] excludeSessionIds)
    {
        message.MessageType = MessageType.Message;
        return base.BroadcastMessageToUserGroupAsync(ownerId, message, excludeSessionIds);
    }

    public string CreateUserSession(long userId, SupportChatRole role)
    {
        var sessionId = base.CreateUserSession(userId);
        _roleSessions[role].TryAdd(sessionId, null);
        return sessionId;
    }

    public void RemoveUserSession(long userId, string sessionId, SupportChatRole role)
    {
        _roleSessions[role].TryRemove(sessionId, out _);
        base.RemoveUserSession(userId, sessionId);
    }

    public async Task NotifyNewMessageSupport(SupportChatMessage message)
    {
        var supportSessions = _roleSessions[SupportChatRole.Support].Keys.ToArray();
        message.MessageType = MessageType.Notification;
        await NotifySessions(message, supportSessions);
    }
}

public enum SupportChatRole
{
    User,
    Support
}