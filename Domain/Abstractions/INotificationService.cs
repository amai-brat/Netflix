using Domain.Entities;

namespace Domain.Abstractions;

public interface INotificationService
{
    Task<CommentNotification?> GetCommentNotificationByCommentIdAsync(long commentId);
    Task<List<CommentNotification>> GetAllUserCommentNotificationsAsync(long userId);
}