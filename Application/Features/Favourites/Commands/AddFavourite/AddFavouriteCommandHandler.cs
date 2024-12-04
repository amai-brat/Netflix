using Application.Cqrs.Commands;
using Application.Repositories;

namespace Application.Features.Favourites.Commands.AddFavourite;

internal class AddFavouriteCommandHandler(
    IFavouriteContentRepository favouriteContentRepository) : ICommandHandler<AddFavouriteCommand>
{
    public async Task Handle(AddFavouriteCommand request, CancellationToken cancellationToken)
    {
        await favouriteContentRepository.AddFavouriteContentAsync(request.ContentId, request.UserId);
    }
}