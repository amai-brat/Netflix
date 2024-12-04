using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Users.Queries.GetFavourites;

internal class GetFavouritesQueryHandler(
    IUserRepository userRepository,
    IFavouriteContentRepository favouriteContentRepository,
    IMapper mapper,
    IReviewRepository reviewRepository) : IQueryHandler<GetFavouritesQuery, GetFavouritesDto>
{
    public async Task<GetFavouritesDto> Handle(GetFavouritesQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == request.UserId);
        if (user is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        var favourites = await favouriteContentRepository.GetWithContentAsync(x => x.UserId == request.UserId);
        var favouriteDtos = mapper.Map<List<FavouriteDto>>(favourites);
        
        foreach (var favouriteDto in favouriteDtos)
        {
            favouriteDto.Score = await reviewRepository.GetScoreByUserAsync(request.UserId, favouriteDto.ContentBase.Id);
        }

        return new GetFavouritesDto { FavouriteDtos = favouriteDtos };
    }
}