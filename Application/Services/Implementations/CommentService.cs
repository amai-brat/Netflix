using Application.Exceptions;
using Application.Exceptions.ErrorMessages;
using Application.Exceptions.Particular;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Entities;

namespace Application.Services.Implementations;

public class CommentService(
    ICommentRepository commentRepository,
    IReviewRepository reviewRepository,
    IUserRepository userRepository
    ): ICommentService
{
    public async Task<long> AssignCommentAsync(string text, long userId, long reviewId)
    {
        if(string.IsNullOrEmpty(text))
            throw new CommentServiceArgumentException(ErrorMessages.CommentMustHaveText, text);
        if(await userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
            throw new CommentServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");
        if (await reviewRepository.GetReviewByFilterAsync(r => r.Id == reviewId) is null)
            throw new CommentServiceArgumentException(ErrorMessages.NotFoundReview, $"{reviewId}");
            
        return await commentRepository.AssignCommentAsync(new Comment()
        {
            Text = text,
            UserId = userId,
            ReviewId = reviewId,
            WrittenAt = DateTimeOffset.UtcNow,
            CommentNotification = new CommentNotification()
            {
                Readed = false
            }
        });
    }
    public async Task<Comment> DeleteCommentByIdAsync(long id)
    {
        var comment = await commentRepository.GetCommentByIdAsync(id);

        if (comment == null) 
        {
            throw new CommentServiceArgumentException(ErrorMessages.NotFoundComment, nameof(id));
        }

        comment = commentRepository.Remove(comment);

        await commentRepository.SaveChangesAsync();
        return comment;
    }
}
