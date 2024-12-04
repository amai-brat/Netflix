namespace Application.Dto;

[Obsolete("CQRS")]
public class FavouriteDto
{
    public DateTimeOffset AddedAt { get; set; }
    public int? Score { get; set; }
    public ContentDto ContentBase { get; set; } = null!;
}

[Obsolete("CQRS")]
public class ContentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PosterUrl { get; set; }
}