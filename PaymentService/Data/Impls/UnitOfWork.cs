using PaymentService.Data.Abstractions;

namespace PaymentService.Data.Impls;

public class UnitOfWork(
    AppDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}