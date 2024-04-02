using Domain.Dtos;
using Domain.Entities;

namespace Domain.Abstractions;

public interface ISubscriptionService
{
    public Task<Subscription> AddSubscriptionAsync(NewSubscriptionDto dto);
    public Task<Subscription> DeleteSubscriptionAsync(int subscriptionId);
    public Task<Subscription> EditSubscriptionAsync(EditSubscriptionDto dto);
}