using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IContentService
    {
        Task<ContentBase?> GetContentByIdAsync(long id);
        Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter);
        Task AddFavouriteAsync(long contentId, long userId);
        Task RemoveFavouriteAsync(long contentId, long userId);
    }
}
