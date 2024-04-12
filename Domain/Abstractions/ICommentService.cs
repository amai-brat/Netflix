using Domain.Dtos;

namespace Domain.Abstractions;

public interface ICommentService
{
    Task<long> AssignCommentAsync(string text, long userId, long reviewId);
}