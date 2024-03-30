using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IContentRepository
    {
        Task<ContentBase?> GetContentByFilterAsync(Expression<Func<ContentBase, bool>> filter);
        Task<List<ContentBase>> GetContentsByFilterAsync(Expression<Func<ContentBase, bool>> filter);
        Task<MovieContent?> GetMovieContentByFilterAsync(Expression<Func<MovieContent, bool>> filter);
        Task<SerialContent?> GetSerialContentByFilterAsync(Expression<Func<SerialContent, bool>> filter);
    }
}
