using Application.Cqrs.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Comments.Commands.AssignComment;

internal class AssignCommentCommandHandler(
    ICommentRepository commentRepository) : ICommandHandler<AssignCommentCommand, AssignCommentDto>
{
    public async Task<AssignCommentDto> Handle(AssignCommentCommand request, CancellationToken cancellationToken)
    {
        var commentId = await commentRepository.AssignCommentAsync(new Comment
        {
            Text = request.Text,
            UserId = request.UserId,
            ReviewId = request.ReviewId,
            WrittenAt = DateTimeOffset.UtcNow,
            CommentNotification = new CommentNotification()
            {
                Readed = false
            }
        });
        
        return new AssignCommentDto { CommendId = commentId };
    }
}