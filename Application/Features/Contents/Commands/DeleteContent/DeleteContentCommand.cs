using Application.Cqrs.Commands;

namespace Application.Features.Contents.Commands.DeleteContent;

public record DeleteContentCommand(long ContentId) : ICommand;