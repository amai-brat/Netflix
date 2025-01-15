using SupportPermanentS3Service.Repositories;

namespace SupportPermanentS3Service.Data.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}