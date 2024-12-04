namespace Application.Features.Contents.Queries.Streaming.GetMovieContentSteam;

public class GetMovieContentStreamDto
{
    public int ErrorCode { get; set; }
    public string? Error { get; set; }
    public Stream? VideoStream { get; set; }
}