using Domain.Entities;

namespace Domain.Abstractions;

public interface INotificationService
{
    Task SetNotificationReadedAsync(long notificationId);
    Task<CommentNotification?> GetCommentNotificationByCommentIdAsync(long commentId);
    Task<List<CommentNotification>> GetAllUserCommentNotificationsAsync(long userId);
}