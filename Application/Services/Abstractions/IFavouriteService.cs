namespace Application.Services.Abstractions
{
    public interface IFavouriteService
    {
        Task AddFavouriteAsync(long contentId, long userId);
        Task RemoveFavouriteAsync(long contentId, long userId);
    }
}
