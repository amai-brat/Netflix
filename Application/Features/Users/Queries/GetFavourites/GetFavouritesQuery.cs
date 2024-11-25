using Application.Cqrs.Queries;

namespace Application.Features.Users.Queries.GetFavourites;

public record GetFavouritesQuery(long UserId) : IQuery<GetFavouritesDto>;