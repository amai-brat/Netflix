using DataAccess.Repositories.Abstractions;
using Domain.Entities;

namespace DataAccess.Repositories;

public class SubscriptionRepository(AppDbContext appDbContext): ISubscriptionRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;
    public List<Subscription> GetAll()
    {
        return _appDbContext.Subscriptions.ToList();
    }
}