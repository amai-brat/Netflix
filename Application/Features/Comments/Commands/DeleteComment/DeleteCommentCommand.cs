using Application.Cqrs.Commands;
using Domain.Entities;

namespace Application.Features.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(long CommentId) : ICommand<Comment>;