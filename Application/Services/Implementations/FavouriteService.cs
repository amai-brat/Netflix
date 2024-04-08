using Application.Exceptions;
using Application.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using Domain.Abstractions;

namespace Application.Services.Implementations
{
    public class FavouriteService(
        IContentRepository contentRepository,
        IFavouriteContentRepository favouriteContentRepository,
        IUserRepository userRepository
        ) : IFavouriteService
    {
        private readonly IContentRepository _contentRepository = contentRepository; 
        private readonly IFavouriteContentRepository _favouriteContentRepository = favouriteContentRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task AddFavouriteAsync(long contentId, long userId)
        {
            if(await _userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

            if (await _contentRepository.GetContentByFilterAsync(c => c.Id == contentId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundContent, $"{contentId}");

            if ((await _favouriteContentRepository.GetFavouriteContentsByFilterAsync(f => f.UserId == userId && f.ContentId == contentId)).Count != 0)
                throw new FavouriteServiceArgumentException(ErrorMessages.AlreadyFavourite, $"{contentId}");

            await _favouriteContentRepository.AddFavouriteContentAsync(contentId, userId);
        }

        public async Task RemoveFavouriteAsync(long contentId, long userId)
        {
            if (await _userRepository.GetUserByFilterAsync(u => u.Id == userId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundUser, $"{userId}");

            if (await _contentRepository.GetContentByFilterAsync(c => c.Id == contentId) is null)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotFoundContent, $"{contentId}");

            if ((await _favouriteContentRepository.GetFavouriteContentsByFilterAsync(f => f.UserId == userId && f.ContentId == contentId)).Count == 0)
                throw new FavouriteServiceArgumentException(ErrorMessages.NotInFavourite, $"{contentId}");

            await _favouriteContentRepository.RemoveFavouriteContentAsync(contentId, userId);
        }
    }
}
