using DataAccess;
using HotChocolate.Language;

namespace MobileAPI.Types.Genre;

[ExtendObjectType(OperationType.Query)]
public class GenreQuery
{
    [UseProjection]
    public IQueryable<Domain.Entities.Genre> GetGenres([Service] AppDbContext appDbContext)
    {
        return appDbContext.Genres;
    }
}