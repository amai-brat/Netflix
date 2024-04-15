using Application.Exceptions;
using Application.Repositories;
using Application.Services.Abstractions;

namespace Application.Services.Implementations
{
    public class FavouriteService(
        IContentRepository contentRepository,
        IFavouriteContentRepository favouriteContentRepository,
        IUserRepository userRepository
        ) : IFavouriteService
    {
        public async Task AddFavouriteAsync(long contentId, long userId)
        {
            if(await userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

            if (await contentRepository.GetContentByFilterAsync(c => c.Id == contentId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundContent, $"{contentId}");

            if ((await favouriteContentRepository.GetFavouriteContentsByFilterAsync(f => f.UserId == userId && f.ContentId == contentId)).Count != 0)
                throw new FavouriteServiceArgumentException(ErrorMessages.AlreadyFavourite, $"{contentId}");

            await favouriteContentRepository.AddFavouriteContentAsync(contentId, userId);
        }

        public async Task RemoveFavouriteAsync(long contentId, long userId)
        {
            if (await userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

            if (await contentRepository.GetContentByFilterAsync(c => c.Id == contentId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundContent, $"{contentId}");

            if ((await favouriteContentRepository.GetFavouriteContentsByFilterAsync(f => f.UserId == userId && f.ContentId == contentId)).Count == 0)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotInFavourite, $"{contentId}");

            await favouriteContentRepository.RemoveFavouriteContentAsync(contentId, userId);
        }
    }
}
