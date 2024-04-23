using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class GenreRepository(AppDbContext dbContext) : IGenreRepository
{
    public async Task<List<Genre>> GetGenresAsync()
    {
        return await dbContext.Genres.ToListAsync();
    }
}