namespace Application.Services.Abstractions
{
    [Obsolete("CQRS")]
    public interface IFavouriteService
    {
        Task AddFavouriteAsync(long contentId, long userId);
        Task RemoveFavouriteAsync(long contentId, long userId);
    }
}
