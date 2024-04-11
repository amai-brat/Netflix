using Domain.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface ISubscriptionRepository
{
    List<Subscription> GetAll();
}