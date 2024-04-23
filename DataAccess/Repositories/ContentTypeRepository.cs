using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ContentTypeRepository(AppDbContext appDbContext): IContentTypeRepository
{
    public async Task<List<ContentType>> GetContentTypesAsync()
    {
        return await appDbContext.ContentTypes.ToListAsync();
    }
}