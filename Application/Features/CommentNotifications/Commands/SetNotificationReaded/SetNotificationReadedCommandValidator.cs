using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.CommentNotifications.Commands.SetNotificationReaded;

internal class SetNotificationReadedCommandValidator : AbstractValidator<SetNotificationReadedCommand>
{
    private readonly ICommentNotificationRepository _notificationRepository;
    
    public SetNotificationReadedCommandValidator(ICommentNotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;

        RuleFor(x => x.NotificationId)
            .MustAsync(IsNotificationExistAsync)
            .WithMessage(ErrorMessages.NotFoundNotification);
    }

    private async Task<bool> IsNotificationExistAsync(long notificationId, CancellationToken cancellationToken)
    {
        var notif = await _notificationRepository.GetCommentNotificationByFilterAsync(c => c.Id == notificationId);
        return notif is not null;
    }
}