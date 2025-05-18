using Application.Features.Favourites.Commands.RemoveFavourite;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MediatR;
using MobileAPI.Helpers;

namespace MobileAPI.Types.FavouriteContent;

[ExtendObjectType(OperationType.Mutation)]
public class FavouriteContentMutation
{
    //[Authorize]
    public async Task<bool> RemoveFromFavourite(
        [Argument] long contentId,
        [Service] IMediator mediator,
        [Service] IHttpContextAccessor accessor)
    {
        //var userId = accessor.HttpContext!.GetUserId();
        var userId = 1;
        await mediator.Send(new RemoveFavouriteCommand(contentId, userId));
        return true;
    }
}