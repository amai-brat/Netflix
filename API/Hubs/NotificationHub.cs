using System.Collections.Concurrent;
using Application.Services.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class NotificationHub(
    INotificationService notificationService
    ): Hub
{
    private readonly INotificationService _notificationService = notificationService;
    private static readonly ConcurrentDictionary<long, string> Connections = [];

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst("id")?.Value;
        
        Connections.TryRemove(long.Parse(userId!), out _);
        
        return Task.FromResult(base.OnDisconnectedAsync(exception));
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst("id")?.Value;
        
        Connections.TryAdd(long.Parse(userId!), Context!.ConnectionId);
        
        return Task.FromResult(base.OnConnectedAsync());
    }
    
    public async Task NotifyAboutCommentAsync(long commentId)
    {
        var userId = long.Parse(Context.User?.FindFirst("id")?.Value!);

        var commentNotification = await _notificationService.GetCommentNotificationByCommentIdAsync(commentId);

        if (commentNotification is null)
            return;
        
        if (userId == commentNotification.Comment.Review.UserId) return;
        
        var connectionId = Connections[commentNotification.Comment.Review.UserId];
        await Clients.Client(connectionId).SendAsync("ReceiveNotification", SetCommentNotification(commentNotification));
    }

    public async Task ReadNotification(long notificationId)
    {
        await _notificationService.SetNotificationReadedAsync(notificationId);
        await Clients.Client(Context.ConnectionId).SendAsync("DeleteNotification", notificationId);
    }
    
    private CommentNotification SetCommentNotification(CommentNotification commentNotification)
    {
        commentNotification.Comment.CommentNotification = null!;
        commentNotification.Comment.Review.Comments = null!;
        commentNotification.Comment.Review.User.Reviews = null!;
        commentNotification.Comment.User.Comments = null!;
        return commentNotification;
    }
}