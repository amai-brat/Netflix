using Domain.Entities;

namespace Application.Repositories;

public interface IGenreRepository
{
    public Task<List<Genre>> GetGenresAsync();
}