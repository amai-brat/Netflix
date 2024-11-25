using Application.Cqrs.Commands;

namespace Application.Features.Favourites.Commands.AddFavourite;

public record AddFavouriteCommand(long ContentId, long UserId) : ICommand;