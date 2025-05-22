using System.Collections.Concurrent;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using SupportAPI.Services.Abstractions;

namespace SupportAPI.Services.Implementations;

public class SupportChatSessionManager: ISupportChatSessionManager
{
    private readonly ConcurrentDictionary<long, List<string>> _userSessions = new();
    private readonly ConcurrentDictionary<string, IServerStreamWriter<SupportChatMessage>?> _sessionStreams = new();
    private readonly ConcurrentDictionary<long, List<string>> _userGroups = new();
    
    public string CreateUserSession(long userId)
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

    public void BindSessionWithStream(string sessionId, IServerStreamWriter<SupportChatMessage> stream)
    {
        if (_sessionStreams.ContainsKey(sessionId))
        {
            _sessionStreams[sessionId] = stream;
        }
    }

    public void RemoveUserSession(long userId, string sessionId)
    {
        if (!_userSessions.TryGetValue(userId, out var value)) 
            return;
        
        value.Remove(sessionId);
        _sessionStreams.TryRemove(sessionId, out _);
        
        if (!value.IsNullOrEmpty())
            return;
        
        _userSessions.TryRemove(userId, out _);
        RemoveUserGroup(userId);
    }

    public void JoinUserSessionGroup(long ownerId, string sessionId)
    {
        if (!_userGroups.TryGetValue(ownerId, out var group)) 
            return;
            
        group.Add(sessionId);
    }

    public void LeaveUserSessionGroup(long ownerId, string sessionId)
    {
        if (_userGroups.TryGetValue(ownerId, out var group))
        {
            group.Remove(sessionId);
        }
    }

    public bool IsSessionBelongToUser(long userId, string sessionId) =>
        _userSessions.TryGetValue(userId, out var userSessions) &&
        userSessions.Any(userSessionId => userSessionId == sessionId);

    public async Task BroadcastMessageToUserGroupAsync(long ownerId, SupportChatMessage message, List<string>? excludeSessionIds = null)
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
    
    private void CreateUserGroup(long userId, string sessionId)
    {
        if (!_userGroups.ContainsKey(userId))
        {
            _userGroups.TryAdd(userId, [sessionId]);
        }
    }

    private void RemoveUserGroup(long userId)
    {
        if (_userGroups.ContainsKey(userId))
        {
            _userGroups.TryRemove(userId, out _);
        }
    }
}