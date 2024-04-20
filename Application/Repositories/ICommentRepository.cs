using Domain.Entities;

namespace Application.Repositories;

public interface ICommentRepository
{
	Task<long> AssignCommentAsync(Comment comment);
	public Task<Comment?> GetCommentByIdAsync(long id);
	public Task SaveChangesAsync();
	public Comment Remove(Comment comment);
}

