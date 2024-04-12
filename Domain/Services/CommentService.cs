using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;

namespace Domain.Services;

public class CommentService(
    ICommentRepository commentRepository,
    IReviewRepository reviewRepository,
    IUserRepository userRepository
    ): ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IReviewRepository _reviewRepository = reviewRepository;

    public async Task<long> AssignCommentAsync(string text, long userId, long reviewId)
    {
        if(string.IsNullOrEmpty(text))
            throw new CommentServiceArgumentException(ErrorMessages.CommentMustHaveText, text);
        if(await _userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
            throw new CommentServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");
        if (await _reviewRepository.GetReviewByFilterAsync(r => r.Id == reviewId) is null)
            throw new CommentServiceArgumentException(ErrorMessages.NotFoundReview, $"{reviewId}");
            
        return await _commentRepository.AssignCommentAsync(new Comment()
        {
            Text = text,
            UserId = userId,
            ReviewId = reviewId,
            CommentNotification = new CommentNotification()
            {
                Readed = false
            }
        });
    }
}