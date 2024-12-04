using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Comments.Commands.DeleteComment;

internal class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentCommandValidator(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;

        RuleFor(x => x.CommentId)
            .MustAsync(IsCommentExistAsync)
            .WithMessage(ErrorMessages.NotFoundComment);
    }

    private async Task<bool> IsCommentExistAsync(long commentId, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        return comment != null;
    }
}