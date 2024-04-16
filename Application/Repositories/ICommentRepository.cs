using Domain.Entities;

namespace Application.Repositories;

public interface ICommentRepository
{
    Task<long> AssignCommentAsync(Comment comment);
}