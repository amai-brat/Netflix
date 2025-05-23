using System.Collections.Concurrent;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using SupportAPI.Services.Abstractions;

namespace SupportAPI.Services.Implementations;

public class ChatSessionManager<T>: IChatSessionManager<T> where T: class
{
    private readonly ConcurrentDictionary<long, ConcurrentDictionary<string, object?>> _userSessions = new();
    private readonly ConcurrentDictionary<string, IServerStreamWriter<T>?> _sessionStreams = new();
    private readonly ConcurrentDictionary<long, ConcurrentDictionary<string, object?>> _userGroups = new();
    
    public virtual string CreateUserSession(long userId)
    {
        var sessionId = Guid.NewGuid().ToString();
        
        _userSessions.GetOrAdd(userId, []);
        
        _userSessions[userId].TryAdd(sessionId, null);
        _sessionStreams.TryAdd(sessionId, null);
        CreateUserGroup(userId, sessionId);
        return sessionId;
    }

    public void BindSessionWithStream(string sessionId, IServerStreamWriter<T>? stream)
    {
        if (_sessionStreams.ContainsKey(sessionId))
        {
            _sessionStreams[sessionId] = stream;
        }
    }

    public virtual void RemoveUserSession(long userId, string sessionId)
    {
        if (!_userSessions.TryGetValue(userId, out var value)) 
            return;
        
        value.TryRemove(sessionId, out _);
        _sessionStreams.TryRemove(sessionId, out _);
        LeaveUserSessionGroup(userId, sessionId);
            
        if (!value.IsNullOrEmpty())
            return;
        
        _userSessions.TryRemove(userId, out _);
    }

    public virtual void JoinUserSessionGroup(long ownerId, string sessionId)
    {
        _userGroups.GetOrAdd(ownerId, []);
        _userGroups[ownerId].TryAdd(sessionId, null);
    }

    public virtual void LeaveUserSessionGroup(long ownerId, string sessionId)
    {
        if (!_userGroups.TryGetValue(ownerId, out var group)) 
            return;
        
        group.TryRemove(sessionId, out _);
        
        if (group.IsNullOrEmpty())
            RemoveUserGroup(ownerId);
    }

    public bool IsSessionBelongToUser(long userId, string sessionId) =>
        _userSessions.TryGetValue(userId, out var userSessions) &&
        userSessions.Any(userSessionId => userSessionId.Key == sessionId);

    public virtual async Task BroadcastMessageToUserGroupAsync(long ownerId, T message, params string[] excludeSessionIds)
    {
        if (_userGroups.TryGetValue(ownerId, out var group))
        {
            foreach (var sessionId in group.ToArray().Where(sId => 
                         excludeSessionIds == null || !excludeSessionIds.Contains(sId.Key)))
            {
                if (!_sessionStreams.TryGetValue(sessionId.Key, out var stream) || stream == null) 
                    continue;
                
                try
                {
                    await stream.WriteAsync(message);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }

    public virtual async Task NotifySessions(T message, params string[] sessionIds)
    {
        foreach (var sessionId in sessionIds)
        {
            if (!_sessionStreams.TryGetValue(sessionId, out var stream) || stream == null) 
                continue;
            
            try
            {
                await stream.WriteAsync(message);
            }
            catch
            {
                // ignored
            }
        }
    }
    
    private void CreateUserGroup(long userId, string sessionId)
    {
        _userGroups.GetOrAdd(userId, []);
        _userGroups[userId].TryAdd(sessionId, null);
    }

    private void RemoveUserGroup(long userId)
    {
        if (_userGroups.ContainsKey(userId))
        {
            _userGroups.TryRemove(userId, out _);
        }
    }
}