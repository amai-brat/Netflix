using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            
            // обновляем проперти базового класса
            dbMovieContent.Name = newMovieContent.Name;
            dbMovieContent.Description = newMovieContent.Description;
            dbMovieContent.Slogan = newMovieContent.Slogan;
            dbMovieContent.PosterUrl = newMovieContent.PosterUrl;
            dbMovieContent.Country = newMovieContent.Country;
            dbMovieContent.ContentType.ContentTypeName = newMovieContent.ContentType.ContentTypeName;
            dbMovieContent.AgeRatings = newMovieContent.AgeRatings;
            dbMovieContent.Ratings = newMovieContent.Ratings;
            dbMovieContent.TrailerInfo = newMovieContent.TrailerInfo;
            dbMovieContent.Budget = newMovieContent.Budget;
            // обновляем проперти класса
            dbMovieContent.MovieLength = newMovieContent.MovieLength;
            dbMovieContent.VideoUrl = newMovieContent.VideoUrl;
            dbMovieContent.ReleaseDate = newMovieContent.ReleaseDate;
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
                .Include(sc => sc.YearRange)
                .FirstOrDefaultAsync(sc => sc.Id == serialContent.Id);
            
            if (serialContentDbVersion is null) throw new Exception("Content not found");
            // обновляем проперти базового класса
            serialContentDbVersion.Name = serialContent.Name;
            serialContentDbVersion.Description = serialContent.Description;
            serialContentDbVersion.Slogan = serialContent.Slogan;
            serialContentDbVersion.PosterUrl = serialContent.PosterUrl;
            serialContentDbVersion.Country = serialContent.Country;
            serialContentDbVersion.ContentType.ContentTypeName = serialContent.ContentType.ContentTypeName;
            serialContentDbVersion.AgeRatings = serialContent.AgeRatings;
            serialContentDbVersion.Ratings = serialContent.Ratings;
            serialContentDbVersion.TrailerInfo = serialContent.TrailerInfo;
            serialContentDbVersion.Budget = serialContent.Budget;
            // обновляем проперти класса
            serialContentDbVersion.YearRange = serialContent.YearRange;
            //обновляем навигационные свойства
            UpdateContentBase(serialContentDbVersion, serialContent);
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
            // нужно проверить что тех полей которые были даны нет в бд, иначе подгружать их
            var existingGenres = appDbContext.Genres.ToList();
            var existingSubscriptions = appDbContext.Subscriptions.ToList();
            var existingPersons = appDbContext.PersonInContents.ToList();
            var existingProfessions = appDbContext.Professions.ToList();
            var existingContentTypes = appDbContext.ContentTypes.ToList();
            // пройдемся по жанрам и если имя такого уже есть в бд, то заменим его на версию в бд
            foreach (var genre in movieContent.Genres)
            {
                if (existingGenres.Any(g => g.Name.Equals(genre.Name)))
                {
                    movieContent.Genres.Remove(genre);
                    movieContent.Genres.Add(existingGenres.First(g => g.Name.Equals(genre.Name)));
                }
            }
            // тоже самое с подписками
            foreach (var subscription in movieContent.AllowedSubscriptions)
            {
                if (existingSubscriptions.Any(s => s.Name.Equals(subscription.Name)))
                {
                    movieContent.AllowedSubscriptions.Remove(subscription);
                    movieContent.AllowedSubscriptions.Add(existingSubscriptions.First(s => s.Name.Equals(subscription.Name)));
                }
            }
            // тоже самое с персонами
            foreach (var person in movieContent.PersonsInContent)
            {
                if (existingPersons.Any(p => p.Name.Equals(person.Name) && p.Profession.ProfessionName.Equals(person.Profession.ProfessionName)))
                {
                    movieContent.PersonsInContent.Remove(person);
                    movieContent.PersonsInContent.Add(existingPersons.First(p => p.Name.Equals(person.Name) && p.Profession.ProfessionName.Equals(person.Profession.ProfessionName)));
                }
            }
            // тоже самое с профессиями
            foreach (var person in movieContent.PersonsInContent)
            {
                if (existingProfessions.Any(p => p.ProfessionName.Equals(person.Profession.ProfessionName)))
                {
                    person.Profession = existingProfessions.First(p => p.ProfessionName.Equals(person.Profession.ProfessionName));
                }
            }
            // тоже самое с типом контента
            if (existingContentTypes.Any(ct => ct.ContentTypeName.Equals(movieContent.ContentType.ContentTypeName)))
            {
                movieContent.ContentType = existingContentTypes.First(ct => ct.ContentTypeName.Equals(movieContent.ContentType.ContentTypeName));
            }
            appDbContext.MovieContents.Add(movieContent);
        }

        public void AddSerialContent(SerialContent serialContent)
        {
            // нужно проверить что тех полей которые были даны нет в бд, иначе подгружать их
            var existingGenres = appDbContext.Genres.ToList();
            var existingSubscriptions = appDbContext.Subscriptions.ToList();
            var existingPersons = appDbContext.PersonInContents.ToList();
            var existingProfessions = appDbContext.Professions.ToList();
            var existingContentTypes = appDbContext.ContentTypes.ToList();
            // TODO: вообще это можно было бы вынести в отдельный метод, т.к. это дублируется в AddMovieContent
            // TODO: исправить изменение коллекции в цикле
            // пройдемся по жанрам и если имя такого уже есть в бд, то заменим его на версию в бд
            for (int i = serialContent.Genres.Count - 1; i >= 0; i--)
            {
                if (existingGenres.Any(g => g.Name.Equals(serialContent.Genres[i].Name)))
                {
                    serialContent.Genres.Add(existingGenres.First(g => g.Name.Equals(serialContent.Genres[i].Name)));
                    serialContent.Genres.Remove(serialContent.Genres[i]);
                }
            }
            // тоже самое с подписками
            for (int i = serialContent.AllowedSubscriptions.Count - 1; i >= 0; i--)
            {
                if (existingSubscriptions.Any(s => s.Name.Equals(serialContent.AllowedSubscriptions[i].Name)))
                {
                    serialContent.AllowedSubscriptions.Add(existingSubscriptions.First(s => s.Name.Equals(serialContent.AllowedSubscriptions[i].Name)));
                    serialContent.AllowedSubscriptions.Remove(serialContent.AllowedSubscriptions[i]);
                }
            }
            // тоже самое с персонами
            for(int i = serialContent.PersonsInContent.Count - 1; i >= 0; i--)
            {
                if (existingPersons.Any(p => p.Name.Equals(serialContent.PersonsInContent[i].Name) && p.Profession.ProfessionName.Equals(serialContent.PersonsInContent[i].Profession.ProfessionName)))
                {
                    serialContent.PersonsInContent.Add(existingPersons.First(p => p.Name.Equals(serialContent.PersonsInContent[i].Name) && p.Profession.ProfessionName.Equals(serialContent.PersonsInContent[i].Profession.ProfessionName)));
                    serialContent.PersonsInContent.Remove(serialContent.PersonsInContent[i]);
                }
            }
            // тоже самое с профессиями
            for(int i = serialContent.PersonsInContent.Count - 1; i >= 0; i--)
            {
                if (existingProfessions.Any(p => p.ProfessionName.Equals(serialContent.PersonsInContent[i].Profession.ProfessionName)))
                {
                    serialContent.PersonsInContent[i].Profession = existingProfessions.First(p => p.ProfessionName.Equals(serialContent.PersonsInContent[i].Profession.ProfessionName));
                }
            }
            // тоже самое с типом контента
            if (existingContentTypes.Any(ct => ct.ContentTypeName.Equals(serialContent.ContentType.ContentTypeName)))
            {
                serialContent.ContentType = existingContentTypes.First(ct => ct.ContentTypeName.Equals(serialContent.ContentType.ContentTypeName));
            }
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
                // TODO: если честно здесь оплошность т.к. логика приложения не должна быть в репозитории
                // надо бы в сервисе это делать, но в данном случае это не критично
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
