using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    private readonly IContentRepository _contentRepository;
    
    public CreateSubscriptionCommandValidator(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SubscriptionErrorMessages.NotValidSubscriptionName)
            .Matches("^[А-Яа-яA-Za-z0-9-_ ]+$").WithMessage(SubscriptionErrorMessages.NotValidSubscriptionName);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(SubscriptionErrorMessages.NotValidSubscriptionDescription);
        
        RuleFor(x => x.MaxResolution)
            .GreaterThan(0).WithMessage(SubscriptionErrorMessages.NotValidSubscriptionMaxResolution);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage(SubscriptionErrorMessages.SubscriptionPriceLessThanZero);

        RuleFor(x => x.AccessibleContentIds)
            .MustAsync(AreContentsExistAsync).WithMessage(SubscriptionErrorMessages.GivenIdOfNonExistingContent);
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