using Application.Dto;
using Domain.Entities;

namespace Application.Services.Abstractions;

public interface ISubscriptionService
{
    public Task<Subscription> AddSubscriptionAsync(NewSubscriptionDto dto);
    public Task<Subscription> DeleteSubscriptionAsync(int subscriptionId);
    public Task<Subscription> EditSubscriptionAsync(EditSubscriptionDto dto);
    public Task<List<AdminSubscriptionsDto>> GetSubscriptionsAsync();
    public Task<List<AdminSubscriptionContentDto>> GetContentsAsync();
}