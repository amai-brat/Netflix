using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IFavouriteService
    {
        Task AddFavouriteAsync(long contentId, long userId);
        Task RemoveFavouriteAsync(long contentId, long userId);
    }
}
