using Application.Cqrs.Queries;
using Application.Features.Subscriptions.Queries.GetAvailableContents;
using Application.Repositories;

namespace Application.Features.Subscriptions.Queries.GetSubscriptions;

internal class GetSubscriptionsQueryHandler(
    ISubscriptionRepository subscriptionRepository) : IQueryHandler<GetSubscriptionsQuery, GetSubscriptionsDto>
{
    public async Task<GetSubscriptionsDto> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionRepository.GetAllSubscriptionsWithAccessibleContentAsync();
        var result = subscriptions
            .Select(subscription => new GetSubscriptionDto()
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Description = subscription.Description,
                MaxResolution = subscription.MaxResolution,
                Price = subscription.Price,
                AccessibleContent = subscription.AccessibleContent
                    .Select(x => new SubscriptionContentDto { Id = x.Id, Name = x.Name })
                    .ToList()
            })
            .ToList();

        return new GetSubscriptionsDto { Dtos = result };
    }
}