using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Repositories;

public interface ICommentNotificationRepository
{
    Task SetNotificationReadedAsync(long id);
    Task<CommentNotification?> GetCommentNotificationByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter);
    Task<List<CommentNotification>> GetAllCommentNotificationsByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter);
}