using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class CommentRepository(AppDbContext appDbContext) : ICommentRepository
	{
		public async Task<Comment?> GetCommentByIdAsync(long id)
		{
			return await appDbContext.Comments.FindAsync(id);
		}

		public async Task<List<Comment>> GetCommentsByReviewIdAsync(long id)
		{
			return await appDbContext.Comments.Where(c => c.ReviewId == id).ToListAsync();
		}

		public Comment Remove(Comment comment)
		{
			return appDbContext.Comments.Remove(comment).Entity;
		}

		public async Task SaveChangesAsync()
		{
			await appDbContext.SaveChangesAsync();
		}
	}
}
