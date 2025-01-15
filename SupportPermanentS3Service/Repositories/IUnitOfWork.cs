namespace SupportPermanentS3Service.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}