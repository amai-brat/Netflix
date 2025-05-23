using System.Collections.Concurrent;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using SupportAPI.Services.Abstractions;

namespace SupportAPI.Services.Implementations;

public class ChatSessionManager<T>: IChatSessionManager<T> where T: class
{
    private readonly ConcurrentDictionary<long, List<string>> _userSessions = new();
    private readonly ConcurrentDictionary<string, IServerStreamWriter<T>?> _sessionStreams = new();
    private readonly ConcurrentDictionary<long, List<string>> _userGroups = new();
    
    public virtual string CreateUserSession(long userId)
    {
        var sessionId = Guid.NewGuid().ToString();
        
        if (!_userSessions.ContainsKey(userId))
        {
            _userSessions.TryAdd(userId, []);
        }
        
        _userSessions[userId].Add(sessionId);
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
        
        value.Remove(sessionId);
        _sessionStreams.TryRemove(sessionId, out _);
        LeaveUserSessionGroup(userId, sessionId);
            
        if (!value.IsNullOrEmpty())
            return;
        
        _userSessions.TryRemove(userId, out _);
    }

    public virtual void JoinUserSessionGroup(long ownerId, string sessionId)
    {
        if (!_userGroups.TryGetValue(ownerId, out var _)) 
            _userGroups.TryAdd(ownerId, []);
            
        _userGroups[ownerId].Add(sessionId);
    }

    public virtual void LeaveUserSessionGroup(long ownerId, string sessionId)
    {
        if (!_userGroups.TryGetValue(ownerId, out var group)) 
            return;
        
        group.Remove(sessionId);
        
        if (group.IsNullOrEmpty())
            RemoveUserGroup(ownerId);
    }

    public bool IsSessionBelongToUser(long userId, string sessionId) =>
        _userSessions.TryGetValue(userId, out var userSessions) &&
        userSessions.Any(userSessionId => userSessionId == sessionId);

    public virtual async Task BroadcastMessageToUserGroupAsync(long ownerId, T message, params string[] excludeSessionIds)
    {
        if (_userGroups.TryGetValue(ownerId, out var group))
        {
            foreach (var sessionId in group.Where(sId => 
                         excludeSessionIds == null || !excludeSessionIds.Contains(sId)))
            {
                if (_sessionStreams.TryGetValue(sessionId, out var stream) && stream != null)
                {
                    await stream.WriteAsync(message);
                }
            }
        }
    }

    public virtual async Task NotifySessions(T message, params string[] sessionIds)
    {
        foreach (var sessionId in sessionIds)
        {
            if (_sessionStreams.TryGetValue(sessionId, out var stream) && stream != null)
            {
                await stream.WriteAsync(message);
            }
        }
    }
    
    private void CreateUserGroup(long userId, string sessionId)
    {
        if (!_userGroups.ContainsKey(userId))
        {
            _userGroups.TryAdd(userId, []);
        }
        _userGroups[userId].Add(sessionId);
    }

    private void RemoveUserGroup(long userId)
    {
        if (_userGroups.ContainsKey(userId))
        {
            _userGroups.TryRemove(userId, out _);
        }
    }
}