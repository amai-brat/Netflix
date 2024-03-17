namespace Domain;

public class SeasonInfo
{
    public int Id { get; set; }

    public SerialContent SerialContent { get; set; } = null!;

    public int SeasonNumber { get; set; }
    public int EpisodesCount { get; set; }
}