using Application.Cqrs.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Subscriptions.Commands.EditSubscription;

internal class EditSubscriptionCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IContentRepository contentRepository) : ICommandHandler<EditSubscriptionCommand, Subscription>
{
    public async Task<Subscription> Handle(EditSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = (await subscriptionRepository.GetSubscriptionWithAccessibleContentAsync(request.SubscriptionId))!;
        
        if (request.NewName is not null)
            subscription.Name = request.NewName;

        if (request.NewDescription is not null)
            subscription.Description = request.NewDescription;

        if (request.NewMaxResolution is not null)
            subscription.MaxResolution = request.NewMaxResolution.Value;

        if (request.NewPrice is not null)
            subscription.Price = request.NewPrice.Value;
        
        if (request.AccessibleContentIdsToAdd != null)
        {
            foreach (var contentId in request.AccessibleContentIdsToAdd)
            {
                var content = await contentRepository.GetContentByIdAsync(contentId);
                if (subscription.AccessibleContent.All(x => x.Id != content!.Id))
                    subscription.AccessibleContent.Add(content!);
            }
        }

        if (request.AccessibleContentIdsToRemove != null)
        {
            foreach (var contentId in request.AccessibleContentIdsToRemove)
            {
                var content = await contentRepository.GetContentByIdAsync(contentId);
                subscription.AccessibleContent.Remove(content!);
            }
        }

        await subscriptionRepository.SaveChangesAsync(cancellationToken);
        return subscription;
    }
}