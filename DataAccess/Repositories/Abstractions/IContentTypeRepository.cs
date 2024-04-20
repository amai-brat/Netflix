using Domain.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface IContentTypeRepository
{
    List<ContentType> GetContentTypes();
}