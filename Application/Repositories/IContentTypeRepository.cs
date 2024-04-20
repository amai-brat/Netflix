using Domain.Entities;

namespace Application.Repositories;

public interface IContentTypeRepository
{
    List<ContentType> GetContentTypes();
}