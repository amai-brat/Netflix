namespace Domain.Entities;

public class MovieContent : ContentBase
{
    public long MovieLength { get; set; }
    public string VideoUrl { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
}