namespace Application.Dto;

public class SectionDto
{
    public string Name { get; set; } = null!;
    public List<SectionContentDto> Contents { get; set; } = [];
}

public class SectionContentDto 
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PosterUrl { get; set; } = null!;
}