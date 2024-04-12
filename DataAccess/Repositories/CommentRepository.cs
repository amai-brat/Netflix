using Domain.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class CommentRepository(AppDbContext appDbContext): ICommentRepository
{
    public async Task<long> AssignCommentAsync(Comment comment)
    {
        var id = (await appDbContext.Comments.AddAsync(comment)).Entity.Id;
        await appDbContext.SaveChangesAsync();
        return id;
    }
}