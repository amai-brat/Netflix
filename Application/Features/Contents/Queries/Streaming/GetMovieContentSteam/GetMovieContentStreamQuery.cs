using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.Streaming.GetMovieContentSteam;

public record GetMovieContentStreamQuery(long UserId, long MovieId, int Resolution) : IQuery<GetMovieContentStreamDto>;