using Domain.Entities;

namespace Domain.Abstractions;

public interface ICommentRepository
{
    Task<long> AssignCommentAsync(Comment comment);
}