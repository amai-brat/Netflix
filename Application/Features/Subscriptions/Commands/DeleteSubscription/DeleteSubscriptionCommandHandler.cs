using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Subscriptions.Commands.DeleteSubscription;

internal class DeleteSubscriptionCommandHandler(
    ISubscriptionRepository subscriptionRepository) : ICommandHandler<DeleteSubcriptionCommand, Subscription>
{
    public async Task<Subscription> Handle(DeleteSubcriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetSubscriptionByIdAsync(request.Id);
        if (subscription is null)
        {
            throw new ArgumentValidationException(
                SubscriptionErrorMessages.SubscriptionNotFound);
        }

        subscription = subscriptionRepository.Remove(subscription);
        await subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return subscription;
    }
}