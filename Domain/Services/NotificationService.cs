using Domain.Abstractions;
using Domain.Entities;
using Domain.Services.ServiceExceptions;

namespace Domain.Services;

public class NotificationService(
    ICommentNotificationRepository commentNotificationRepository,
    IUserRepository userRepository
    ) : INotificationService
{
    private readonly ICommentNotificationRepository _notificationRepository = commentNotificationRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<CommentNotification?> GetCommentNotificationByCommentIdAsync(long commentId) =>
        await _notificationRepository.GetCommentNotificationByFilterAsync(c => c.Comment.Id == commentId);

    public async Task<List<CommentNotification>> GetAllUserCommentNotificationsAsync(long userId)
    {
        if (await _userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
            throw new NotificationServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

        return await _notificationRepository.GetAllCommentNotificationsByFilterAsync(n => 
                n.Comment.Review.UserId == userId && 
                (!n.Readed || n.Comment.WrittenAt.Date > DateTime.UtcNow.AddDays(-10)));
    }
}