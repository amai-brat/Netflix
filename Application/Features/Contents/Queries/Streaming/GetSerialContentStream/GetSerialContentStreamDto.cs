namespace Application.Features.Contents.Queries.Streaming.GetSerialContentStream;

public class GetSerialContentStreamDto
{
    public int ErrorCode { get; set; }
    public string? Error { get; set; }
    public Stream? VideoStream { get; set; }
}