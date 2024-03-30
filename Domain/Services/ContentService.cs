using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
            await _contentRepository.GetContentsByFilterAsync(content =>
                IsContentNameContain(content, filter) &&
                IsContentTypesContain(content, filter) &&
                IsCountryContain(content, filter) &&
                IsContentGenresContains(content, filter) &&
                IsContentYearBetween(content, filter) &&
                IsContentRatingBetween(content, filter)
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

        private bool IsContentNameContain(ContentBase content, Filter filter) => 
            filter.Name == null || content.Name.ToLower().Contains(filter.Name.ToLower());

        private bool IsContentTypesContain(ContentBase content, Filter filter) =>
            filter.Types == null || filter.Types.Any(id => id == content.ContentTypeId);

        private bool IsCountryContain(ContentBase content, Filter filter) =>
            filter.Country == null || (content.Country != null && content.Country.ToLower() == filter.Country.ToLower());

        private bool IsContentGenresContains(ContentBase content, Filter filter) =>
            filter.Genres == null || filter.Genres.All(id => content.Genres.FirstOrDefault(g => g.Id == id) != null);

        private bool IsContentYearBetween(ContentBase content, Filter filter)
        {
            if (content is MovieContent movie)
                return (!filter.ReleaseYearFrom.HasValue || filter.ReleaseYearFrom.Value <= movie.ReleaseDate.Year) &&
                    (!filter.ReleaseYearTo.HasValue || filter.ReleaseYearTo.Value >= movie.ReleaseDate.Year);
            else if (content is SerialContent serial)
                return (!filter.ReleaseYearFrom.HasValue || filter.ReleaseYearFrom.Value <= serial.YearRange.Start.Year) &&
                    (!filter.ReleaseYearTo.HasValue || filter.ReleaseYearTo.Value >= serial.YearRange.End.Year);
            else
                return true;
        }

        private bool IsContentRatingBetween(ContentBase content, Filter filter) =>
            (filter.RatingFrom == null || filter.RatingFrom.Value <= (content.Ratings?.KinopoiskRating ?? 0)) &&
            (filter.RatingTo == null || filter.RatingTo.Value >= (content.Ratings?.KinopoiskRating ?? 0));
    }
}
