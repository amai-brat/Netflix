using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.Streaming.GetMovieContentStreamChunk;

public record GetMovieContentStreamChunkQuery(long UserId, long MovieId, int Resolution) 
    : IQuery<GetMovieContentStreamChunkDto>;