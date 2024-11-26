using System.Linq.Expressions;
using Application.Cqrs.Queries;
using Application.Repositories;
using Application.Services.Extensions;
using Domain.Entities;

namespace Application.Features.Contents.Queries.GetContentsByFilter;

internal class GetContentsByFilterQueryHandler(
    IContentRepository contentRepository) : IQueryHandler<GetContentsByFilterQuery, GetContentsByFilterDto>
{
    public async Task<GetContentsByFilterDto> Handle(GetContentsByFilterQuery request, CancellationToken cancellationToken)
    {
        var list = await contentRepository.GetContentsByFilterAsync(
            IsContentNameContain(request.Filter)
                .CombineExpressions(IsContentTypesContain(request.Filter))
                .CombineExpressions(IsCountryContain(request.Filter))
                .CombineExpressions(IsContentGenresContains(request.Filter))
                .CombineExpressions(IsContentYearBetween(request.Filter))
                .CombineExpressions(IsContentRatingBetween(request.Filter))
        );

        return new GetContentsByFilterDto { Contents = list };
    }
    
    private Expression<Func<ContentBase, bool>> IsContentNameContain(Filter filter) =>
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

    private Expression<Func<ContentBase, bool>> IsContentRatingBetween(Filter filter) =>
        content =>
            (filter.RatingFrom == null || filter.RatingFrom.Value <=
                (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating)) &&
            (filter.RatingTo == null ||
             filter.RatingTo.Value >= (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating));
}