using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories.Abstractions;

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

        public MovieContent UpdateMovieContent(MovieContent movieContent)
        {
            var movieContentDbVersion = appDbContext.MovieContents
                .Include(mc => mc.AllowedSubscriptions)
                .Include(mc => mc.Genres)
                .Include(mc => mc.PersonsInContent)
                .Include(mc => mc.Ratings)
                .Include(mc => mc.TrailerInfo)
                .Include(mc => mc.Budget)
                .Include(mc => mc.ContentType)
                .First(mc => mc.Id == movieContent.Id);
            if (movieContentDbVersion is null) throw new Exception("Movie content not found");
            appDbContext.Entry(movieContentDbVersion).CurrentValues.SetValues(movieContent);
            
            movieContentDbVersion.ContentType.ContentTypeName = movieContent.ContentType.ContentTypeName;
            movieContentDbVersion.Ratings.KinopoiskRating = movieContent.Ratings.KinopoiskRating;
            movieContentDbVersion.Ratings.ImdbRating = movieContent.Ratings.ImdbRating;
            movieContentDbVersion.Ratings.ImdbRating = movieContent.Ratings.LocalRating;
                
            movieContentDbVersion.Ratings = movieContent.Ratings;
            movieContentDbVersion.Genres = movieContent.Genres;
            movieContentDbVersion.PersonsInContent = movieContent.PersonsInContent;
            movieContentDbVersion.AllowedSubscriptions = movieContent.AllowedSubscriptions;
            movieContentDbVersion.Budget = movieContent.Budget;
            movieContentDbVersion.TrailerInfo = movieContent.TrailerInfo;
            return appDbContext.MovieContents.Update(movieContentDbVersion).Entity;
            
        }
        public SerialContent UpdateSerialContent(SerialContent serialContent)
        {
            return appDbContext.SerialContents.Update(serialContent).Entity;
        }
        public ContentBase DeleteContent(long id)
        {
            var content = appDbContext.ContentBases.Find(id);
            if (content is null) throw new Exception("Content not found");
            return appDbContext.ContentBases.Remove(content).Entity;
        }

        public MovieContent AddMovieContent(MovieContent movieContent)
        {
            return appDbContext.MovieContents.Add(movieContent).Entity;
        }

        public SerialContent AddSerialContent(SerialContent serialContent)
        {
            return appDbContext.SerialContents.Add(serialContent).Entity;
        }
        

        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}
