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

        public async Task UpdateMovieContent(MovieContent newMovieContent)
        {
            var dbMovieContent = await appDbContext.MovieContents
                .Include(mc => mc.AllowedSubscriptions)
                .Include(mc => mc.Genres)
                .Include(mc => mc.PersonsInContent)
                    .ThenInclude(person => person.Profession)
                .Include(mc => mc.ContentType)
                .FirstOrDefaultAsync(mc => mc.Id == newMovieContent.Id);
            if (dbMovieContent is null) throw new Exception("Content not found");
            // обновляем проперти
            appDbContext.Entry(dbMovieContent).CurrentValues.SetValues(newMovieContent);
            // обновляем навигационные свойства
            UpdateContentBase(dbMovieContent, newMovieContent);
        }
        
        public async Task UpdateSerialContent(SerialContent serialContent)
        {
            var serialContentDbVersion = await appDbContext.SerialContents
                .Include(sc => sc.AllowedSubscriptions)
                .Include(sc => sc.Genres)
                .Include(sc => sc.PersonsInContent)
                    .ThenInclude(person => person.Profession)
                .Include(sc => sc.ContentType)
                .Include(sc => sc.SeasonInfos)
                    .ThenInclude(s => s.Episodes)
                        .ThenInclude(e => e.SeasonInfo)
                .Include(sc => sc.YearRange)
                .FirstOrDefaultAsync(sc => sc.Id == serialContent.Id);
            
            if (serialContentDbVersion is null) throw new Exception("Content not found");
            // обновляем проперти
            appDbContext.Entry(serialContentDbVersion).CurrentValues.SetValues(serialContent);
            //обновляем навигационные свойства
            UpdateContentBase(serialContentDbVersion, serialContent);
            serialContentDbVersion.YearRange.Start = serialContent.YearRange.Start;
            serialContentDbVersion.YearRange.End = serialContent.YearRange.End;
            
            //лучше было бы сделать в одном методе, но так понятнее(я пожалел об этом)
            UpdateSeasonInfo(serialContentDbVersion, serialContent);
            UpdateEpisodes(serialContentDbVersion, serialContent);
        }
        //обязательно должен идти после обновления сезонов
        private void UpdateEpisodes(SerialContent serialContentDbVersion,
            SerialContent serialContentNew)
        {
            // удаляем все эпизоды, которых нет в новом контенте
            serialContentDbVersion.SeasonInfos.ForEach(si =>
            {
                si.Episodes.RemoveAll(e =>
                    !serialContentNew.SeasonInfos.First(s => s.SeasonNumber == si.SeasonNumber)
                        .Episodes.Any(se => se.EpisodeNumber == e.EpisodeNumber));
            });
            // добавляем новые эпизоды или обновляет инфу о старых
            foreach (var season in serialContentNew.SeasonInfos)
            {
                var dbSeason = serialContentDbVersion.SeasonInfos.First(si => si.SeasonNumber == season.SeasonNumber);
                foreach (var episode in season.Episodes)
                {
                    var dbEpisode = dbSeason.Episodes.FirstOrDefault(ep => ep.EpisodeNumber == episode.EpisodeNumber);
                    // если есть такая серия в сезоне в версии бд, то просто обновляем ее внутрянку
                    if (dbEpisode != null)
                    {
                        dbEpisode.VideoUrl = episode.VideoUrl;
                    }
                    // если такой серии нет, то добавляем ее
                    else
                    {
                        dbSeason.Episodes.Add(episode);
                    }
                }
            }
        }

        private void UpdateSeasonInfo(SerialContent serialContentDbVersion,
            SerialContent serialContent)
        {
            // удаляем все сезоны, которых нет в новом контенте
            serialContentDbVersion.SeasonInfos.RemoveAll(si => 
                !serialContent.SeasonInfos.Any(s => s.SeasonNumber == si.SeasonNumber));
            // добавляем все сезоны, которые появились в новом контенте
            foreach (var season in serialContent.SeasonInfos)
            {
                // если такой сезон уже есть, идем дальше
                if (serialContentDbVersion.SeasonInfos.Any(si => si.SeasonNumber == season.SeasonNumber))
                {
                    continue;
                }
                // добавляем новый
                serialContentDbVersion.SeasonInfos.Add(season);
            }
        }

        public ContentBase DeleteContent(long id)
        {
            var content = appDbContext.ContentBases.Find(id);
            if (content is null) throw new Exception("Такого контента нет");
            return appDbContext.ContentBases.Remove(content).Entity;
        }

        public void AddMovieContent(MovieContent movieContent)
        {
            appDbContext.MovieContents.Add(movieContent);
        }

        public void AddSerialContent(SerialContent serialContent)
        {
            appDbContext.SerialContents.Add(serialContent);
        }

        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }
        private void UpdateContentBase(ContentBase dbContent,
            ContentBase newContent)
        {
            var existingGenres = appDbContext.Genres.ToList();
            var existingSubscriptions = appDbContext.Subscriptions.ToList();
            var existingPersons = appDbContext.PersonInContents.ToList();
            var existingProfessions = appDbContext.Professions.ToList();
            // обновляем навигационные свойства
            dbContent.ContentType.ContentTypeName = newContent.ContentType.ContentTypeName;
            UpdateGenres(dbContent, newContent.Genres, existingGenres);
            UpdateSubscriptions(dbContent, newContent.AllowedSubscriptions, existingSubscriptions);
            UpdatePersonsAndProfession(dbContent, newContent.PersonsInContent, existingPersons, existingProfessions);
        }
        // обязательно должны быть загружены все персоны и профессии
        private void UpdatePersonsAndProfession(ContentBase dbContent,
            List<PersonInContent> newContentPersonsInContent,
            List<PersonInContent> existingPersons,
            List<Profession> existingProfessions)
        {
            // удалем персоны, которых нет в измененном контенте
            dbContent.PersonsInContent.RemoveAll(p => 
                !newContentPersonsInContent.Any(np => 
                    np.Name.Equals(p.Name) && np.Profession.ProfessionName.Equals(p.Profession.ProfessionName)));
            // добавляем персоны, которые появились в измененном контенте
            foreach (var newPerson in newContentPersonsInContent)
            {
                // если такой уже есть, идем дальше
                if (dbContent.PersonsInContent.Any(p => 
                        p.Name.Equals(newPerson.Name) &&
                        p.Profession.ProfessionName.Equals(newPerson.Profession.ProfessionName)))
                {
                    continue;
                }
                // если такого нет, но он уже есть в бд, то добавляем версию из бд
                if (existingPersons.Any(ep => ep.Name.Equals(newPerson.Name) &&
                                              ep.Profession.ProfessionName.Equals(newPerson.Profession.ProfessionName)))
                {
                    dbContent.PersonsInContent.Add(existingPersons.First(ep => 
                        ep.Name.Equals(newPerson.Name) &&
                        ep.Profession.ProfessionName.Equals(newPerson.Profession.ProfessionName)));
                    continue;
                }
                // если такого нет и в бд тоже нет, то добавляем полностью нового
                // не забываем что если у него профессии нет в бд, то нужно добавить и её
                if (existingProfessions.Any(p => p.ProfessionName.Equals(newPerson.Profession.ProfessionName)))
                {
                    var profession = existingProfessions.First(p => p.ProfessionName.Equals(newPerson.Profession.ProfessionName));
                    newPerson.Profession = profession;
                    dbContent.PersonsInContent.Add(newPerson);
                }
                else
                {
                    dbContent.PersonsInContent.Add(newPerson);
                    existingProfessions.Add(newPerson.Profession);
                }
            }
        }
        // подписки должны быть предварительно загружены.
        private void UpdateSubscriptions(ContentBase dbContent,
            List<Subscription> newContentAllowedSubscriptions,
            List<Subscription> existingSubscriptions)
        {
            dbContent.AllowedSubscriptions.RemoveAll(g =>
                !newContentAllowedSubscriptions.Any(newSub => newSub.Name.Equals(g.Name)));

            foreach (var subscription in newContentAllowedSubscriptions)
            {
                // если в существующих подписках уже есть тот который добавляем, то идем дальше
                if (dbContent.AllowedSubscriptions.Any(dbs => dbs.Name.Equals(subscription.Name)))
                {
                    continue;
                }
                // если в существующих её нет, но он уже есть в тех которые в бд, то 
                // добавляем версию из бд
                var firstOrDefault = existingSubscriptions
                    .FirstOrDefault(s => s.Name.Equals(subscription.Name));
                if (firstOrDefault != null)
                {
                    dbContent.AllowedSubscriptions.Add(firstOrDefault);
                }
                // если в существующих её нет и её нет в бд, то выкидываем исключение. 
                // админ может добавлять только из существующих(для добавления подписок у него отдельная вкладка)
                else
                {
                    throw new Exception("Вы не можете добавить свою подписку к фильму");
                }
            }
        }
        // жанры должны быть предварительно загружены.
        private void UpdateGenres(ContentBase dbContent,
            List<Genre> newContentGenres,
            List<Genre> existingGenres)
        {
            dbContent.Genres.RemoveAll(g => !newContentGenres.Any(newGenre => newGenre.Name.Equals(g.Name)));

            foreach (var genre in newContentGenres)
            {
                // если в существующих жанрах уже есть тот который добавляем, то идем дальше
                if (dbContent.Genres.Any(dbg => dbg.Name.Equals(genre.Name)))
                {
                    continue;
                }
                // если в существующих его нет, но он уже есть в тех которые в бд, то 
                // добавляем версию из бд
                var firstOrDefault = existingGenres.FirstOrDefault(g => g.Name.Equals(genre.Name));
                if (firstOrDefault != null)
                {
                    dbContent.Genres.Add(firstOrDefault);
                    continue;
                }
                // если в существующих его нет и его нет в бд, то добавляем полностьюб новый жанр.
                // TODO: хорошо было бы сделать [ConcurrencyCheck] на имя жанра, т.к. в этом этапе может быть такое
                // что пока программа работала кто-то добавил такой жанр а локальный кеш не обновлялся
                // и в бд всё же добавится дубликат
                // (или будет исключение в зависимости от конфигураций)
                // но в нашем проекте такое маловероятно. это еще и время тратит, в будущем можно допилить
                dbContent.Genres.Add(genre);
                existingGenres.Add(genre);
            }
        }
    }
}
