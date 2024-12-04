using Application.Cqrs.Commands;
using Application.Repositories;

namespace Application.Features.Favourites.Commands.RemoveFavourite;

internal class RemoveFavouriteCommandHandler(
    IFavouriteContentRepository favouriteContentRepository) : ICommandHandler<RemoveFavouriteCommand>
{
    public async Task Handle(RemoveFavouriteCommand request, CancellationToken cancellationToken)
    {
        await favouriteContentRepository.RemoveFavouriteContentAsync(request.ContentId, request.UserId);
    }
}