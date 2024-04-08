using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IFavouriteContentRepository
    {
        Task<List<FavouriteContent>> GetFavouriteContentsByFilterAsync(Expression<Func<FavouriteContent, bool>> filter);
        Task AddFavouriteContentAsync(long contentId, long userId);
        Task RemoveFavouriteContentAsync(long contentId, long userId);
    }
}
