namespace Domain.Entities;

public class Episode
{
	public long Id { get; set; }

	public int EpisodeNumber { get; set; }

	public string VideoUrl { get; set; } = null!;

	public SeasonInfo SeasonInfo { get; set; } = null!;
}
