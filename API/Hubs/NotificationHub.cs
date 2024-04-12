using Domain.Abstractions;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class NotificationHub(
    INotificationService notificationService
    ): Hub
{
    private readonly INotificationService _notificationService = notificationService;
    private static readonly Dictionary<long, string> Connections = [];
    
    public override Task OnConnectedAsync()
    {
        var userId = Context?.User?.FindFirst("id")?.Value;
        Connections.Add(long.Parse(userId), Context.ConnectionId);
        
        return base.OnConnectedAsync();
    }
    
    public async Task NotifyAboutCommentAsync(long commentId)
    {
        var commentNotification = await _notificationService.GetCommentNotificationByCommentIdAsync(commentId);

        if (commentNotification is null)
            return;
        
        var connectionId = Connections[commentNotification.Comment.Review.UserId];
        await Clients.Client(connectionId).SendAsync("ReceiveNotification", commentNotification);
    }
}