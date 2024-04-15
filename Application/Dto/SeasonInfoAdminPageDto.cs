namespace Application.Dto;

public class SeasonInfoAdminPageDto
{
    public int SeasonNumber { get; set; }
    public List<EpisodeAdminPageDto> Episodes { get; set; } = null!;
}