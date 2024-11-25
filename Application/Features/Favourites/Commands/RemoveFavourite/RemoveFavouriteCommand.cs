using Application.Cqrs.Commands;

namespace Application.Features.Favourites.Commands.RemoveFavourite;

public record RemoveFavouriteCommand(long ContentId, long UserId) : ICommand;