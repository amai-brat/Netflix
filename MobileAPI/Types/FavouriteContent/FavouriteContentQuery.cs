using System.Linq.Expressions;
using Application.Features.Users.Queries.GetFavourites;
using DataAccess;
using Domain.Entities;
using HotChocolate.Authorization;
using HotChocolate.Language;
using Microsoft.EntityFrameworkCore;
using MobileAPI.Helpers;

namespace MobileAPI.Types.FavouriteContent;

[ExtendObjectType(OperationType.Query)]
public class FavouriteContentQuery
{
    [Authorize]
    [UsePaging]
    [UseProjection]
    public async Task<IQueryable<FavouriteDto>> GetFavouriteContents(
        [Argument] FavouriteFilter filter, 
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext!.GetUserId();
        var favourites = context.FavouriteContents
            .Where(f => f.UserId == userId)
            .Include(f => f.Content)
                .ThenInclude(c => c.Reviews)
            .Where(IsContentNameContain(filter));
            
        favourites = OrderByFavouriteFilter(favourites, filter);

        var dtoContents = favourites.Select(f => new FavouriteDto()
        {
            ContentBase = new ContentDto()
            {
                Id = f.ContentId,
                Name = f.Content.Name,
                PosterUrl = f.Content.PosterUrl,
            },
            AddedAt = f.AddedAt,
            Score = f.Content.Reviews == null ? 
                null : 
                f.Content.Reviews.FirstOrDefault(r => userId == r.UserId) == null ?
                    null :
                    f.Content.Reviews.FirstOrDefault(r => userId == r.UserId)!.Score,
        });
        if (filter.SortBy is FavouriteSortBy.UserRatingAsc or FavouriteSortBy.UserRatingDesc)
            dtoContents = OrderByFavouriteFilter(await dtoContents.ToListAsync(), filter).AsQueryable();
        return dtoContents;
    }
    
    private static Expression<Func<Domain.Entities.FavouriteContent, bool>> IsContentNameContain(FavouriteFilter filter) =>
        favourite => filter.Name == null || favourite.Content.Name.ToLower().Contains(filter.Name.ToLower());
    
    private static IQueryable<Domain.Entities.FavouriteContent> OrderByFavouriteFilter(IQueryable<Domain.Entities.FavouriteContent> favourites, FavouriteFilter filter) =>
        !filter.SortBy.HasValue ?
            favourites
            : filter.SortBy.Value switch
            {
                FavouriteSortBy.PublicRatingDesc => favourites.OrderByDescending(f => f.Content.Ratings == null ? 0 : f.Content.Ratings.KinopoiskRating),
                FavouriteSortBy.PublicRatingAsc => favourites.OrderBy(f => f.Content.Ratings == null ? 10 : f.Content.Ratings.KinopoiskRating),
                FavouriteSortBy.DateDesc => favourites.OrderByDescending(f =>
                    f.Content is MovieContent ? ((MovieContent)f.Content).ReleaseDate :
                    f.Content is SerialContent ? ((SerialContent)f.Content).YearRange.Start : DateOnly.MinValue),
                FavouriteSortBy.DateAsc => favourites.OrderByDescending(f =>
                    f.Content is MovieContent ? ((MovieContent)f.Content).ReleaseDate :
                    f.Content is SerialContent ? ((SerialContent)f.Content).YearRange.Start : DateOnly.MaxValue),
                FavouriteSortBy.AddedDateDesc => favourites.OrderByDescending(f => f.AddedAt),
                FavouriteSortBy.AddedDateAsc => favourites.OrderBy(f => f.AddedAt),
                FavouriteSortBy.TitleAsc => favourites.OrderBy(f => f.Content.Name),
                FavouriteSortBy.TitleDesc => favourites.OrderByDescending(f => f.Content.Name),
                _ => favourites
            };
    
    private static IEnumerable<FavouriteDto> OrderByFavouriteFilter(IEnumerable<FavouriteDto> favourites, FavouriteFilter filter) =>
        !filter.SortBy.HasValue ?
            favourites
            : filter.SortBy.Value switch
            {
                FavouriteSortBy.UserRatingDesc => favourites.OrderByDescending(f => f.Score ?? -1),
                FavouriteSortBy.UserRatingAsc => favourites.OrderBy(f => f.Score ?? 11),
                _ => favourites
            };
}