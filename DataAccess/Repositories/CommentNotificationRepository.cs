using System.Linq.Expressions;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class CommentNotificationRepository(AppDbContext appDbContext) : ICommentNotificationRepository
{
    public async Task<CommentNotification?> GetCommentNotificationByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter) =>
        await appDbContext.CommentNotifications
            .Include(c => c.Comment)
                .ThenInclude(c => c.Review)
            .SingleOrDefaultAsync(filter);

    public async Task<List<CommentNotification>> GetAllCommentNotificationsByFilterAsync(
        Expression<Func<CommentNotification, bool>> filter) =>
        await appDbContext.CommentNotifications
            .Include(c => c.Comment)
                .ThenInclude(c => c.Review)
            .Where(filter)
            .ToListAsync();
}