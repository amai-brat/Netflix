using Application.Cqrs.Queries;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.CommentNotifications.Queries.GetCommentNotification;

internal class GetCommentNotificationQueryHandler(
    ICommentNotificationRepository notificationRepository) : IQueryHandler<GetCommentNotificationQuery, CommentNotification?>
{
    public async Task<CommentNotification?> Handle(GetCommentNotificationQuery request, CancellationToken cancellationToken)
    {
        return await notificationRepository.GetCommentNotificationByFilterAsync(c => c.Comment.Id == request.CommentId);
    }
}