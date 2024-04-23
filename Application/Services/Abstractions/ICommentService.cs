using Domain.Entities;

namespace Application.Services.Abstractions;

public interface ICommentService
{
    Task<long> AssignCommentAsync(string text, long userId, long reviewId);
   Task<Comment> DeleteCommentByIdAsync(long id);

}