using Application.Cqrs.Commands;
using Application.Repositories;

namespace Application.Features.CommentNotifications.Commands.SetNotificationReaded;

internal class SetNotificationReadedCommandHandler(
    ICommentNotificationRepository notificationRepository) : ICommandHandler<SetNotificationReadedCommand>
{
    public async Task Handle(SetNotificationReadedCommand request, CancellationToken cancellationToken)
    {
        await notificationRepository.SetNotificationReadedAsync(request.NotificationId);
    }
}