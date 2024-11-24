using Domain.Entities;

namespace Application.Features.CommentNotifications.Queries.GetAllUserCommentNotifications;

public class GetAllUserCommentNotificationsDto
{
    public List<CommentNotification> Notifications { get; set; } = null!;
}