using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.Streaming.GetSerialContentStream;

public record GetSerialContentStreamQuery(long UserId, long SerialId, int Season, int Episode, int Resolution) 
    : IQuery<GetSerialContentStreamDto>;