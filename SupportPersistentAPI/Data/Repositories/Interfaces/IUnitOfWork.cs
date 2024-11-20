namespace SupportPersistentAPI.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
