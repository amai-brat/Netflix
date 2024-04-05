using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ContentRepository(AppDbContext appDbContext) : IContentRepository
    {
        public async Task<ContentBase?> GetContentByFilterAsync(Expression<Func<ContentBase, bool>> filter) =>
            await appDbContext.ContentBases.Include(c => c.ContentType)
                .Include(c => c.Genres)
                .Include(c => c.PersonsInContent)
                    .ThenInclude(p => p.Profession)
                .SingleOrDefaultAsync(filter);

        public async Task<List<ContentBase>> GetContentsByFilterAsync(Expression<Func<ContentBase, bool>> filter) => 
            await appDbContext.ContentBases.Where(filter)
                .ToListAsync();

        public async Task<MovieContent?> GetMovieContentByFilterAsync(Expression<Func<MovieContent, bool>> filter) =>
            await appDbContext.MovieContents.Include(m => m.AllowedSubscriptions)
                .SingleOrDefaultAsync(filter);

        public async Task<SerialContent?> GetSerialContentByFilterAsync(Expression<Func<SerialContent, bool>> filter) => 
            await appDbContext.SerialContents.Include(s => s.AllowedSubscriptions)
                .Include(s => s.SeasonInfos)
                    .ThenInclude(s => s.Episodes)
                .SingleOrDefaultAsync(filter);
    }
}
