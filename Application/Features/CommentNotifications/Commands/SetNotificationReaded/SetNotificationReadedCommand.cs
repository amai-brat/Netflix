using Application.Cqrs.Commands;

namespace Application.Features.CommentNotifications.Commands.SetNotificationReaded;

public record SetNotificationReadedCommand(long NotificationId) : ICommand;
