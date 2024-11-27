namespace Application.Features.Contents.Queries.Streaming.GetMovieContentStreamChunk;

public class GetMovieContentStreamChunkDto
{
    public int ErrorCode { get; set; }
    public string? Error { get; set; }
    public Stream? VideoStream { get; set; }
}