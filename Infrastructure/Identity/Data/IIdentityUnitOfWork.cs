namespace Infrastructure.Identity.Data;

public interface IIdentityUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}