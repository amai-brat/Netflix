namespace Application.Identity;

public interface IIdentityUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}