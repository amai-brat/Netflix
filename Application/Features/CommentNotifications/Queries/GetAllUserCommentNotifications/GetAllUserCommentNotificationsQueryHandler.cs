using Application.Cqrs.Queries;
using Application.Repositories;

namespace Application.Features.CommentNotifications.Queries.GetAllUserCommentNotifications;

internal class GetAllUserCommentNotificationsQueryHandler(
    ICommentNotificationRepository notificationRepository) : IQueryHandler<GetAllUserCommentNotificationsQuery, GetAllUserCommentNotificationsDto>
{
    public async Task<GetAllUserCommentNotificationsDto> Handle(GetAllUserCommentNotificationsQuery request, CancellationToken cancellationToken)
    {
        var ntfs = await notificationRepository.GetAllCommentNotificationsByFilterAsync(n => 
            n.Comment.Review.UserId == request.UserId && 
            (!n.Readed || n.Comment.WrittenAt > DateTimeOffset.UtcNow.AddDays(-10)));
        
        return new GetAllUserCommentNotificationsDto { Notifications = ntfs };
    }
}