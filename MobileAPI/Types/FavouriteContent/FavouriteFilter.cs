namespace MobileAPI.Types.FavouriteContent;

public class FavouriteFilter
{
    public string? Name { get; set; }
    public FavouriteSortBy? SortBy { get; set; }
}

public enum FavouriteSortBy
{
    AddedDateDesc,
    AddedDateAsc,
    UserRatingDesc,
    UserRatingAsc,
    PublicRatingDesc,
    PublicRatingAsc,
    DateDesc,
    DateAsc,
    TitleAsc,
    TitleDesc,
}