using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ContentService(
        IContentRepository contentRepository,
        IFavouriteContentRepository favouriteContentRepository
        ): IContentService
    {
        private readonly IContentRepository _contentRepository = contentRepository;
        private readonly IFavouriteContentRepository _favouriteContentRepository = favouriteContentRepository;

        public async Task AddFavouriteAsync(long contentId, long userId)
        {
            await _favouriteContentRepository.AddFavouriteContnentAsync(contentId, userId);
        }

        public async Task<ContentBase?> GetContentByIdAsync(long id) => 
            await _contentRepository.GetContentByFilterAsync(c => c.Id == id);

        public async Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter) =>
            await _contentRepository.GetContentsByFilterAsync(content =>
                IsContentNameContain(content, filter) &&
                IsContentTypesContain(content, filter) &&
                IsContentGenresContains(content, filter) &&
                IsContentYearBetween(content, filter) &&
                IsContentRatingBetween(content, filter)
            );

        public async Task RemoveFavouriteAsync(long contentId, long userId)
        {
            await _favouriteContentRepository.RemoveFavouriteContnentAsync(contentId, userId);
        }

        private bool IsContentNameContain(ContentBase content, Filter filter) => 
            filter.Name == null || content.Name.Contains(filter.Name);

        private bool IsContentTypesContain(ContentBase content, Filter filter) =>
            filter.Types == null || filter.Types.Any(id => id == content.ContentType.Id);

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
            (filter.ReleaseYearFrom == null || filter.ReleaseYearFrom.Value <= (content.Ratings?.KinopoiskRating ?? -1)) &&
            (filter.ReleaseYearTo == null || filter.ReleaseYearTo.Value >= (content.Ratings?.KinopoiskRating ?? -1));
    }
}
