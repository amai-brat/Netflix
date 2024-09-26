using Domain.Entities;

namespace Application.Services.Abstractions;

public interface INotificationService
{
    Task SetNotificationReadedAsync(long notificationId);
    Task<CommentNotification?> GetCommentNotificationByCommentIdAsync(long commentId);
    Task<List<CommentNotification>> GetAllUserCommentNotificationsAsync(long userId);
}