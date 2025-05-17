using System.Linq.Expressions;
using Application.Services.Extensions;
using DataAccess;
using Domain.Entities;
using HotChocolate.Language;

namespace MobileAPI.Types.Content;

[ExtendObjectType(OperationType.Query)]
public class ContentQuery
{
    [UsePaging]
    [UseProjection]
    public IQueryable<ContentBase> GetContents([Argument] Filter filter, [Service] AppDbContext context)
    {
        var contents = context.ContentBases.Where(
            IsContentNameContain(filter)
                .CombineExpressions(IsContentTypesContain(filter))
                .CombineExpressions(IsCountryContain(filter))
                .CombineExpressions(IsContentGenresContains(filter))
                .CombineExpressions(IsContentYearBetween(filter))
                .CombineExpressions(IsContentRatingBetween(filter)));
        contents = OrderByFilter(contents, filter);
        return contents;
    }
    
    private static Expression<Func<ContentBase, bool>> IsContentNameContain(Filter filter) =>
        content => filter.Name == null || content.Name.ToLower().Contains(filter.Name.ToLower());

    private static Expression<Func<ContentBase, bool>> IsContentTypesContain(Filter filter) =>
        content => filter.Types == null || filter.Types.Count == 0 ||
                   filter.Types.Any(id => id == content.ContentTypeId);

    private static Expression<Func<ContentBase, bool>> IsCountryContain(Filter filter) =>
        content => filter.Country == null ||
                   (content.Country != null && content.Country.ToLower() == filter.Country.ToLower());

    private static Expression<Func<ContentBase, bool>> IsContentGenresContains(Filter filter) =>
        content => filter.Genres == null || filter.Genres.Count == 0 ||
                   filter.Genres.All(id => content.Genres.FirstOrDefault(g => g.Id == id) != null);

    private static Expression<Func<ContentBase, bool>> IsContentYearBetween(Filter filter) =>
        content => content is MovieContent
            ? (!filter.ReleaseYearFrom.HasValue ||
               filter.ReleaseYearFrom.Value <= ((MovieContent)content).ReleaseDate.Year) &&
              (!filter.ReleaseYearTo.HasValue || filter.ReleaseYearTo.Value >= ((MovieContent)content).ReleaseDate.Year)
            : content is SerialContent
                ? (!filter.ReleaseYearFrom.HasValue ||
                   filter.ReleaseYearFrom.Value <= ((SerialContent)content).YearRange.Start.Year) &&
                  (!filter.ReleaseYearTo.HasValue ||
                   filter.ReleaseYearTo.Value >= ((SerialContent)content).YearRange.End.Year)
                : true;

    private static Expression<Func<ContentBase, bool>> IsContentRatingBetween(Filter filter) =>
        content =>
            (filter.RatingFrom == null || filter.RatingFrom.Value <=
                (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating)) &&
            (filter.RatingTo == null ||
             filter.RatingTo.Value >= (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating));

    private static IQueryable<ContentBase> OrderByFilter(IQueryable<ContentBase> contents, Filter filter) =>
        !filter.SortBy.HasValue ?
            contents
            : filter.SortBy.Value switch
            {
                SortBy.RatingDesc => contents.OrderByDescending(c => c.Ratings == null ? 0 : c.Ratings.KinopoiskRating),
                SortBy.RatingAsc => contents.OrderBy(c => c.Ratings == null ? 10 : c.Ratings.KinopoiskRating),
                SortBy.DateDesc => contents.OrderByDescending(c =>
                    c is MovieContent ? ((MovieContent)c).ReleaseDate :
                    c is SerialContent ? ((SerialContent)c).YearRange.Start : DateOnly.MinValue),
                SortBy.DateAsc => contents.OrderByDescending(c =>
                    c is MovieContent ? ((MovieContent)c).ReleaseDate :
                    c is SerialContent ? ((SerialContent)c).YearRange.Start : DateOnly.MaxValue),
                SortBy.TitleAsc => contents.OrderBy(c => c.Name),
                SortBy.TitleDesc => contents.OrderByDescending(c => c.Name),
                _ => contents
            };
}