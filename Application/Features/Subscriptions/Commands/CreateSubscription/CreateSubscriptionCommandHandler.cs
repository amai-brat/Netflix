using Application.Cqrs.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Subscriptions.Commands.CreateSubscription;

internal class CreateSubscriptionCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IContentRepository contentRepository) : ICommandHandler<CreateSubscriptionCommand, Subscription>
{
    public async Task<Subscription> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var contents = new List<ContentBase>();
        foreach (var contentId in request.AccessibleContentIds)
        {
            var content = await contentRepository.GetContentByIdAsync(contentId);
            contents.Add(content!);
        }

        var result = await subscriptionRepository.AddAsync(new Subscription
        {
            Name = request.Name,
            Description = request.Description,
            MaxResolution = request.MaxResolution,
            AccessibleContent = contents,
            Price = request.Price
        });

        await subscriptionRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}