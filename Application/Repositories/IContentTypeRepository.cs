using Domain.Entities;

namespace Application.Repositories;

public interface IContentTypeRepository
{
    Task<List<ContentType>> GetContentTypesAsync();
}