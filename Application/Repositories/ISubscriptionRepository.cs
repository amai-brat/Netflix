using Domain.Entities;

namespace Application.Repositories;

public interface ISubscriptionRepository
{
    public Task<List<Subscription>> GetAllSubscriptionsAsync();
    public Task<List<Subscription>> GetAllSubscriptionsWithAccessibleContentAsync();
    public Task<Subscription> AddAsync(Subscription subscription);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId);
    public Task<Subscription?> GetSubscriptionWithAccessibleContentAsync(int subscriptionId);
    public Subscription Remove(Subscription subscription);
}