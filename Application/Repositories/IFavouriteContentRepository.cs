using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IFavouriteContentRepository
    {
        Task<List<FavouriteContent>> GetFavouriteContentsByFilterAsync(Expression<Func<FavouriteContent, bool>> filter);
        Task AddFavouriteContentAsync(long contentId, long userId);
        Task RemoveFavouriteContentAsync(long contentId, long userId);
        Task<List<FavouriteContent>> GetWithContentAsync(Expression<Func<FavouriteContent, bool>> filter);
    }
}
