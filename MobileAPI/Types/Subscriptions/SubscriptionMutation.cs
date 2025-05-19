using System.Text;
using System.Text.Json;
using Application.Exceptions.Base;
using Domain.Entities;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MobileAPI.Exceptions;

namespace MobileAPI.Types.Subscriptions;

[ExtendObjectType(OperationType.Mutation)]
public class SubscriptionMutation
{
    [Authorize]
    public async Task<UserSubscription> BuySubscription(
        BuySubscriptionInput input,
        [Service] ILogger<SubscriptionMutation> logger,
        [Service] IHttpClientFactory clientFactory)
    {
        try
        {
            var client = clientFactory.CreateClient("SubscriptionService");
            var response = await client.PostAsJsonAsync("subscription/buySubscription", input);

            if ((int)response.StatusCode / 100 == 4)
            {
                var dto = await response.Content.ReadFromJsonAsync<SubscriptionServiceErrorDto>();
                throw new ArgumentValidationException(dto?.Message ?? "Некорректные данные");
            }

            var result = await response.Content.ReadFromJsonAsync<UserSubscription>();
            return result!;
        }
        catch (ArgumentValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogWarning("Error during mutation {Mutation}: {Message}", nameof(BuySubscription), e.Message);
            throw new ServiceUnavailableException("SubscritpionService");
        }
    }

    [Authorize]
    public async Task<CancelSubscriptionPayload> CancelSubscription(
        CancelSubscriptionInput input,
        [Service] ILogger<SubscriptionMutation> logger,
        [Service] IHttpClientFactory clientFactory)
    {
        try
        {
            var client = clientFactory.CreateClient("SubscriptionService");
            var req = new HttpRequestMessage(HttpMethod.Delete, "subscription/cancelSubscription")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { subscriptionId = input.SubscriptionId }),
                    Encoding.UTF8,
                    "application/json")
            };
            var response = await client.SendAsync(req);

            if ((int)response.StatusCode / 100 == 4)
            {
                var dto = await response.Content.ReadFromJsonAsync<SubscriptionServiceErrorDto>();
                throw new ArgumentValidationException(dto?.Message ?? "Некорректные данные");
            }

            return new CancelSubscriptionPayload(true);
        }
        catch (ArgumentValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogWarning("Error during mutation {Mutation}: {Message}", nameof(CancelSubscription), e.Message);
            throw new ServiceUnavailableException("SubscritpionService");
        }
    }
}