using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ContentService(
        IContentRepository contentRepository
        ): IContentService
    {
        private readonly IContentRepository _contentRepository = contentRepository;
        private readonly HashSet<int> resolutions = [480, 720, 1080, 1440, 2160];

        public async Task<ContentBase?> GetContentByIdAsync(long id) => 
            await _contentRepository.GetContentByFilterAsync(c => c.Id == id);

        public async Task<MovieContent?> GetMovieContentByIdAsync(long id) =>
            await _contentRepository.GetMovieContentByFilterAsync(c => c.Id == id);

        public async Task<SerialContent?> GetSerialContentByIdAsync(long id) => 
            await _contentRepository.GetSerialContentByFilterAsync(c => c.Id == id);

        public async Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter) =>
            await _contentRepository.GetContentsByFilterAsync(
                IsContentNameContain(filter)
                .CombineExpressions(IsContentTypesContain(filter))
                .CombineExpressions(IsCountryContain(filter))
                .CombineExpressions(IsContentGenresContains(filter))
                .CombineExpressions(IsContentYearBetween(filter))
                .CombineExpressions(IsContentRatingBetween(filter))
            );

        public async Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, int subscriptionId)
        {
            var movie = await _contentRepository.GetMovieContentByFilterAsync(m => m.Id == movieId);
            if (movie is null)
                throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{movieId}");
            if (!resolutions.Contains(resolution))
                throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");
            if (!movie.AllowedSubscriptions.Select(s => s.Id).Contains(subscriptionId))
                throw new ContentServiceNotPermittedException(ErrorMessages.UserDoesNotHavePermissionBySubscription);
            
            return movie.VideoUrl.Replace("resolution", resolution.ToString());
        }

        public async Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution, int subscriptionId)
        {
            var serial = await _contentRepository.GetSerialContentByFilterAsync(s => s.Id == serialId);
            if (serial is null)
                throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{serialId}");

            if (!resolutions.Contains(resolution))
                throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");

            if (!serial.SeasonInfos.Select(s => s.SeasonNumber)
                .Contains(season))
                throw new ContentServiceArgumentException(ErrorMessages.NotFoundSeason, $"{season}");

            if (!serial.SeasonInfos.Single(s => s.SeasonNumber == season).Episodes
                .Select(e => e.EpisodeNumber)
                .Contains(episode))
                throw new ContentServiceArgumentException(ErrorMessages.NotFoundEpisode, $"{episode}");

            if (!serial.AllowedSubscriptions.Select(s => s.Id)
                .Contains(subscriptionId))
                throw new ContentServiceNotPermittedException(ErrorMessages.UserDoesNotHavePermissionBySubscription);

            return serial.SeasonInfos.Single(s => s.SeasonNumber == season).Episodes
                .Single(e => e.EpisodeNumber == episode).VideoUrl
                .Replace("resolution", resolution.ToString());
        }

        private Expression<Func<ContentBase, bool>> IsContentNameContain(Filter filter) => 
            content => filter.Name == null || content.Name.ToLower().Contains(filter.Name.ToLower());

        private Expression<Func<ContentBase, bool>> IsContentTypesContain(Filter filter) =>
            content => filter.Types == null || filter.Types.Count == 0 || filter.Types.Any(id => id == content.ContentTypeId);

        private Expression<Func<ContentBase, bool>> IsCountryContain(Filter filter) =>
            content => filter.Country == null || (content.Country != null && content.Country.ToLower() == filter.Country.ToLower());

        private Expression<Func<ContentBase, bool>> IsContentGenresContains(Filter filter) =>
            content => filter.Genres == null || filter.Genres.Count == 0 || filter.Genres.All(id => content.Genres.FirstOrDefault(g => g.Id == id) != null);

        private Expression<Func<ContentBase, bool>> IsContentYearBetween(Filter filter) =>
            content => content is MovieContent ? 
                            (!filter.ReleaseYearFrom.HasValue || filter.ReleaseYearFrom.Value <= ((MovieContent)content).ReleaseDate.Year) &&
                            (!filter.ReleaseYearTo.HasValue || filter.ReleaseYearTo.Value >= ((MovieContent)content).ReleaseDate.Year) :
                        content is SerialContent ?
                            (!filter.ReleaseYearFrom.HasValue || filter.ReleaseYearFrom.Value <= ((SerialContent)content).YearRange.Start.Year) &&
                            (!filter.ReleaseYearTo.HasValue || filter.ReleaseYearTo.Value >= ((SerialContent)content).YearRange.End.Year) :
                        true;
        

        private Expression<Func<ContentBase, bool>> IsContentRatingBetween(Filter filter) => 
            content =>
                (filter.RatingFrom == null || filter.RatingFrom.Value <= (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating)) &&
                (filter.RatingTo == null || filter.RatingTo.Value >= (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating));
    }
}
