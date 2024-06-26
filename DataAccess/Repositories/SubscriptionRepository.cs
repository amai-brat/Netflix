using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class SubscriptionRepository(AppDbContext dbContext) : ISubscriptionRepository
{
    public async Task<List<Subscription>> GetAllSubscriptionsAsync()
    {
        return await dbContext.Subscriptions.ToListAsync();
    }

    public async Task<List<Subscription>> GetAllSubscriptionsWithAccessibleContentAsync()
    {
        return await dbContext.Subscriptions
            .Include(x => x.AccessibleContent)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Subscription> AddAsync(Subscription subscription)
    {
        var entry = await dbContext.Subscriptions.AddAsync(subscription);
        return entry.Entity;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await dbContext.Subscriptions.FindAsync(subscriptionId);
    }

    public async Task<Subscription?> GetSubscriptionWithAccessibleContentAsync(int subscriptionId)
    {
        return await dbContext.Subscriptions
            .Include(x => x.AccessibleContent)
            .SingleOrDefaultAsync(x => x.Id == subscriptionId);
    }
    public Subscription Remove(Subscription subscription)
    {
        return dbContext.Subscriptions.Remove(subscription).Entity;
    }
}