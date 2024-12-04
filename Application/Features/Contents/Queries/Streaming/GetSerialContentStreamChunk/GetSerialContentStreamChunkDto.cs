namespace Application.Features.Contents.Queries.Streaming.GetSerialContentStreamChunk;

public class GetSerialContentStreamChunkDto
{
    public int ErrorCode { get; set; }
    public string? Error { get; set; }
    public Stream? VideoStream { get; set; }
}