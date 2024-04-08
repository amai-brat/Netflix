using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.Repositories.Abstractions
{
    public interface IContentRepository
    {
        Task<ContentBase?> GetContentByFilterAsync(Expression<Func<ContentBase, bool>> filter);
        Task<List<ContentBase>> GetContentsByFilterAsync(Expression<Func<ContentBase, bool>> filter);
        Task<MovieContent?> GetMovieContentByFilterAsync(Expression<Func<MovieContent, bool>> filter);
        Task<SerialContent?> GetSerialContentByFilterAsync(Expression<Func<SerialContent, bool>> filter);
        MovieContent UpdateMovieContent(MovieContent movieContent);
        SerialContent UpdateSerialContent(SerialContent serialContent);
        ContentBase DeleteContent(long id);
        MovieContent AddMovieContent(MovieContent movieContent);
        SerialContent AddSerialContent(SerialContent serialContent);
        Task SaveChangesAsync();
        
        
    }
}
