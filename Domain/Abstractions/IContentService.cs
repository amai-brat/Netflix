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
        Task<MovieContent?> GetMovieContentByIdAsync(long id);
        Task<SerialContent?> GetSerialContentByIdAsync(long id);
        Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter);
        Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, int subscriptionId);
        Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution, int subscriptionId);
    }
}
