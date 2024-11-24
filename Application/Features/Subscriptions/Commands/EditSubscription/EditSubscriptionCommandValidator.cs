using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Subscriptions.Commands.EditSubscription;

internal class EditSubscriptionCommandValidator : AbstractValidator<EditSubscriptionCommand>
{
    private readonly IContentRepository _contentRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public EditSubscriptionCommandValidator(
        IContentRepository contentRepository, 
        ISubscriptionRepository subscriptionRepository)
    {
        _contentRepository = contentRepository;
        _subscriptionRepository = subscriptionRepository;

        RuleFor(x => x.SubscriptionId)
            .MustAsync(IsSubscriptionToEditExistsAsync)
            .WithMessage(SubscriptionErrorMessages.SubscriptionNotFound);
            
        RuleFor(x => x.NewName)
            .Matches("^[А-Яа-яA-Za-z0-9-_ ]+$")
            .When(x => x.NewName is not null)
            .WithMessage(SubscriptionErrorMessages.NotValidSubscriptionName);

        RuleFor(x => x.NewDescription)
            .NotEmpty()
            .When(x => x.NewDescription is not null)
            .WithMessage(SubscriptionErrorMessages.NotValidSubscriptionDescription);
        
        RuleFor(x => x.NewMaxResolution)
            .GreaterThan(0)
            .When(x => x.NewMaxResolution is not null)
            .WithMessage(SubscriptionErrorMessages.NotValidSubscriptionMaxResolution);

        RuleFor(x => x.NewPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.NewPrice is not null)
            .WithMessage(SubscriptionErrorMessages.SubscriptionPriceLessThanZero);

        RuleFor(x => x.AccessibleContentIdsToAdd)
            .MustAsync(AreContentsExistAsync!)
            .When(x => x.AccessibleContentIdsToAdd is not null)
            .WithMessage(SubscriptionErrorMessages.GivenIdOfNonExistingContent);
        
        RuleFor(x => x.AccessibleContentIdsToRemove)
            .MustAsync(AreContentsExistAsync!)
            .When(x => x.AccessibleContentIdsToRemove is not null)
            .WithMessage(SubscriptionErrorMessages.GivenIdOfNonExistingContent);
    }

    private async Task<bool> IsSubscriptionToEditExistsAsync(int subscriptionId, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionWithAccessibleContentAsync(subscriptionId);
        return subscription is not null;
    }
    
    private async Task<bool> AreContentsExistAsync(List<long> contentIds, CancellationToken cancellationToken)
    {
        foreach (var contentId in contentIds)
        {
            var content = await _contentRepository.GetContentByIdAsync(contentId);
            if (content == null)
            {
                return false;
            }
        }

        return true;
    }
}