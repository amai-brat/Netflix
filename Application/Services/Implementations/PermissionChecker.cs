using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Entities;

namespace Application.Services.Implementations;

public class PermissionChecker(
    IContentRepository contentRepository,
    IUserRepository userRepository) : IPermissionChecker
{
    public async Task<bool> IsContentAllowedForUserAsync(long contentId, long userId)
    {
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == userId);
        var userSubscriptions = user?
            .UserSubscriptions?
            .Where(x => x.Status == UserSubscriptionStatus.Completed)
            .Select(s => s.SubscriptionId)
            .ToList();
        
        return await IsContentAllowedForAnySubscriptionAsync(contentId, userSubscriptions);
    }
    
    private async Task<bool> IsContentAllowedForAnySubscriptionAsync(long contentId, List<int>? subscriptionIds)
    {
        var content = await contentRepository.GetContentWithAllowedSubscriptionsByIdAsync(contentId);
        if (content == null || subscriptionIds == null || subscriptionIds.Count == 0)
        {
            return false;
        }

        return content.AllowedSubscriptions.Any(s => subscriptionIds.Contains(s.Id));
    }
}