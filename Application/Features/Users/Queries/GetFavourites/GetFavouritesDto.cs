namespace Application.Features.Users.Queries.GetFavourites;

public class GetFavouritesDto
{
    public List<FavouriteDto> FavouriteDtos { get; set; } = null!;
}

public class FavouriteDto
{
    public DateTimeOffset AddedAt { get; set; }
    public int? Score { get; set; }
    public ContentDto ContentBase { get; set; } = null!;
}

public class ContentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PosterUrl { get; set; }
}