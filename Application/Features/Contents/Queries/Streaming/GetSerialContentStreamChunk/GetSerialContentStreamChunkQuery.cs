using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.Streaming.GetSerialContentStreamChunk;

public record GetSerialContentStreamChunkQuery(long UserId, long SerialId, int Season, int Episode, int Resolution)  
    : IQuery<GetSerialContentStreamChunkDto>;