using System.Linq.Expressions;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class CommentNotificationRepository(AppDbContext appDbContext) : ICommentNotificationRepository
{
    public async Task SetNotificationReadedAsync(long id)
    {
        var notification = await appDbContext.CommentNotifications.SingleAsync(c => c.Id == id);
        notification.Readed = true;
        appDbContext.CommentNotifications.Update(notification);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<CommentNotification?> GetCommentNotificationByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter) =>
        await appDbContext.CommentNotifications
            .Include(c => c.Comment)
                .ThenInclude(c => c.Review)
                    .ThenInclude(r => r.User)
            .Include(c => c.Comment)
                .ThenInclude(c => c.User)
            .SingleOrDefaultAsync(filter);

    public async Task<List<CommentNotification>> GetAllCommentNotificationsByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter) =>
        await appDbContext.CommentNotifications
            .Include(c => c.Comment)
                .ThenInclude(c => c.Review)
                    .ThenInclude(r => r.User)
            .Include(c => c.Comment)
                .ThenInclude(c => c.User)
            .Where(filter)
            .ToListAsync();
}