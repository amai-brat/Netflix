namespace Application.Dto;

[Obsolete("CQRS")]
public class SeasonInfoAdminPageDto
{
    public int SeasonNumber { get; set; }
    public List<EpisodeAdminPageDto> Episodes { get; set; } = null!;
}