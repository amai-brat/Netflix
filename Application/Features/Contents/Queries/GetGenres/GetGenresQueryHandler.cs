using Application.Cqrs.Queries;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Contents.Queries.GetGenres;

internal class GetGenresQueryHandler(
    IGenreRepository genreRepository,
    IMapper mapper) : IQueryHandler<GetGenresQuery, GetGenresDto>
{
    public async Task<GetGenresDto> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await genreRepository.GetGenresAsync();
        return new GetGenresDto { GenreDtos = mapper.Map<List<GenreDto>>(genres) };
    }
}