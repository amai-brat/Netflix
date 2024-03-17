namespace Domain;

public class MovieContent : ContentBase
{
    public long MovieLength { get; set; }
    public DateOnly ReleaseDate { get; set; }
}