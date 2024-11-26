using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.GetGenres;

public record GetGenresQuery : IQuery<GetGenresDto>;