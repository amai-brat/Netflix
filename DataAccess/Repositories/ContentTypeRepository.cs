using Application.Repositories;
using Domain.Entities;

namespace DataAccess.Repositories;

public class ContentTypeRepository(AppDbContext appDbContext): IContentTypeRepository
{
    public List<ContentType> GetContentTypes()
    {
        return appDbContext.ContentTypes.ToList();
    }
}