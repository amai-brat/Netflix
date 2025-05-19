using Domain.Entities;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MobileAPI.Exceptions;

namespace MobileAPI.Types.Subscriptions;

[ExtendObjectType(OperationType.Query)]
public class SubscriptionQuery
{
    [Authorize]
    public async Task<IEnumerable<UserSubscription>> UserSubscriptions(
        [Service] ILogger<SubscriptionQuery> logger,
        [Service] IHttpClientFactory clientFactory)
    {
        try
        {
            var client = clientFactory.CreateClient("SubscriptionService");
            var response = await client.GetAsync("subscription/getCurrentUserSubscriptions");
            response.EnsureSuccessStatusCode();
        
            var result = await response.Content.ReadFromJsonAsync<List<UserSubscription>>();
            return result!;
        }
        catch (Exception e)
        {
            logger.LogWarning("Error during query {Query}: {Message}", nameof(UserSubscriptions), e.Message);
            throw new ServiceUnavailableException("SubscritpionService");
        }
    } 
    
    public async Task<IEnumerable<Subscription>> Subscriptions(
        [Service] ILogger<SubscriptionQuery> logger,
        [Service] IHttpClientFactory clientFactory)
    {
        try
        {
            var client = clientFactory.CreateClient("SubscriptionService");
            var response = await client.GetAsync("subscription/getAllSubscriptions");
            response.EnsureSuccessStatusCode();
        
            var result = await response.Content.ReadFromJsonAsync<List<Subscription>>();
            return result!;
        }
        catch (Exception e)
        {
            logger.LogWarning("Error during query {Query}: {Message}", nameof(Subscriptions), e.Message);
            throw new ServiceUnavailableException("SubscritpionService");
        }
    } 
}