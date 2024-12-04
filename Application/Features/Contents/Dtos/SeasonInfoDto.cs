namespace Application.Features.Contents.Dtos;

public class SeasonInfoDto
{
    public int SeasonNumber { get; set; }
    public List<EpisodeDto> Episodes { get; set; } = null!;
}