using Domain.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class CommentRepository(AppDbContext appDbContext): ICommentRepository
{
    public async Task<long> AssignCommentAsync(Comment comment)
    {
        await appDbContext.Comments.AddAsync(comment);
        await appDbContext.SaveChangesAsync();
        return comment.Id;
    }
}