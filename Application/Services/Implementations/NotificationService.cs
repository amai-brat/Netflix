using Application.Exceptions.ErrorMessages;
using Application.Exceptions.Particular;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Entities;

namespace Application.Services.Implementations;

public class NotificationService(
    ICommentNotificationRepository commentNotificationRepository,
    IUserRepository userRepository
    ) : INotificationService
{
    private readonly ICommentNotificationRepository _notificationRepository = commentNotificationRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task SetNotificationReadedAsync(long notificationId)
    {
        if (await _notificationRepository.GetCommentNotificationByFilterAsync(c => c.Id == notificationId) is null)
            throw new NotificationServiceArgumentException(ErrorMessages.NotFoundNotification, $"{notificationId}");

        await _notificationRepository.SetNotificationReadedAsync(notificationId);
    }

    public async Task<CommentNotification?> GetCommentNotificationByCommentIdAsync(long commentId) =>
        await _notificationRepository.GetCommentNotificationByFilterAsync(c => c.Comment.Id == commentId);

    public async Task<List<CommentNotification>> GetAllUserCommentNotificationsAsync(long userId)
    {
        if (await _userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
            throw new NotificationServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

        return await _notificationRepository.GetAllCommentNotificationsByFilterAsync(n => 
                n.Comment.Review.UserId == userId && 
                (!n.Readed || n.Comment.WrittenAt > DateTimeOffset.UtcNow.AddDays(-10)));
    }
}