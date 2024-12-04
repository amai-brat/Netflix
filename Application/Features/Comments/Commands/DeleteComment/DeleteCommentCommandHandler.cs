using Application.Cqrs.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Comments.Commands.DeleteComment;

internal class DeleteCommentCommandHandler(
    ICommentRepository commentRepository) : ICommandHandler<DeleteCommentCommand, Comment>
{
    public async Task<Comment> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetCommentByIdAsync(request.CommentId);
        
        comment = commentRepository.Remove(comment!);

        await commentRepository.SaveChangesAsync();
        return comment;
    }
}