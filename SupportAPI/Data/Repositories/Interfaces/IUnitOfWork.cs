namespace SupportAPI.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
