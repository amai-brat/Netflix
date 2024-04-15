using Application.Repositories;
using Application.Services.Abstractions;

namespace DataAccess.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}