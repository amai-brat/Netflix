using Application.Exceptions;
using Application.Exceptions.Base;
using DataAccess;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity.Data;

public class IdentityUnitOfWork(
    AppDbContext appDbContext, 
    IdentityDbContext identityDbContext,
    ILogger<IdentityUnitOfWork> logger) : IIdentityUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await appDbContext.SaveChangesAsync(cancellationToken);
            await identityDbContext.SaveChangesAsync(cancellationToken);
                
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError("Failed to save changes with {error}", ex.Message);
            throw new BusinessException(ex.Message);
        }
    }
}