using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Abstractions;

public interface ICommentNotificationRepository
{
    Task<CommentNotification?> GetCommentNotificationByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter);
    Task<List<CommentNotification>> GetAllCommentNotificationsByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter);
}