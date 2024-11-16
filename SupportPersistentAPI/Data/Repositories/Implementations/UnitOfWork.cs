using SupportPersistentAPI.Data.Repositories.Interfaces;

namespace SupportPersistentAPI.Data.Repositories.Implementations
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
