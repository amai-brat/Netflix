using Application.Cqrs.Commands;
using Application.Repositories;

namespace Application.Features.Contents.Commands.DeleteContent;

internal class DeleteContentCommandHandler(
    IContentRepository contentRepository) : ICommandHandler<DeleteContentCommand>
{
    public async Task Handle(DeleteContentCommand request, CancellationToken cancellationToken)
    {
        contentRepository.DeleteContent(request.ContentId);
        await contentRepository.SaveChangesAsync();
    }
}