using SupportAPI.Services.Implementations;

namespace SupportAPI.Services.Abstractions;

public interface ISupportChatSessionManager<T>: IChatSessionManager<T> where T : class
{
    string CreateUserSession(long userId, SupportChatRole role);
    void RemoveUserSession(long userId, string sessionId, SupportChatRole role);
    Task NotifyNewMessageSupport(T message);
}