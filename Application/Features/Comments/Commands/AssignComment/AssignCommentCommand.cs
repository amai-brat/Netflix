using Application.Cqrs.Commands;

namespace Application.Features.Comments.Commands.AssignComment;

public record AssignCommentCommand(string Text, long UserId, long ReviewId) : ICommand<AssignCommentDto>;