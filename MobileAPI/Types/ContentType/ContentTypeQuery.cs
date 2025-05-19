using DataAccess;
using HotChocolate.Language;

namespace MobileAPI.Types.ContentType;

[ExtendObjectType(OperationType.Query)]
public class ContentTypeQuery
{
    [UseProjection]
    public IQueryable<Domain.Entities.ContentType> GetContentTypes([Service] AppDbContext appDbContext)
    {
        return appDbContext.ContentTypes;
    }
}