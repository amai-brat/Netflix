namespace Domain;

public class SeasonInfo
{
    public int Id { get; set; }

	public int SeasonNumber { get; set; }

	public SerialContent SerialContent { get; set; } = null!;

    public List<Episode> Episodes { get; set; } = null!;
}