using Application.Cqrs.Queries;
using Application.Features.Contents.Dtos;

namespace Application.Features.Contents.Queries.Admin.GetMovieContent;

public record GetMovieContentQuery(long MovieId) : IQuery<MovieContentDto>;