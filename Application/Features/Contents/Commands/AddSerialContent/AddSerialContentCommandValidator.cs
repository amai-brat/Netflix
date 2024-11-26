using Application.Features.Contents.Dtos;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Contents.Commands.AddSerialContent;

internal class AddSerialContentCommandValidator : AbstractValidator<AddSerialContentCommand>
{
    public AddSerialContentCommandValidator(ISubscriptionRepository subscriptionRepository)
    {
        RuleFor(x => x.ContentDto)
            .SetValidator(new SerialContentDtoValidator(subscriptionRepository));
    }
}

public class SerialContentDtoValidator : AbstractValidator<SerialContentDto>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SerialContentDtoValidator(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name must be not empty")
            .MaximumLength(100)
            .WithMessage("Name must be less than 100 characters");
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
        RuleFor(x => x.PosterUrl)
            .NotEmpty();
        RuleFor(x => x.Slogan)
            .MaximumLength(50);
        RuleFor(x => x.Country)
            .MaximumLength(50);
        RuleFor(x => x.ContentType)
            .NotEmpty();
        RuleFor(x => x.Genres)
            .NotEmpty();
        RuleFor(x => x.PersonsInContent)
            .NotEmpty();
        RuleFor(x => x.AllowedSubscriptions)
            .NotEmpty();
        RuleFor(x => x.SeasonInfos)
            .NotEmpty();
        RuleFor(x => x.AgeRating)
            .ChildRules(ageRating =>
            {
                ageRating.RuleFor(ar => ar!.Age).LessThanOrEqualTo(21);
                ageRating.RuleFor(ar => ar!.AgeMpaa).Length(0, 7); 
            }).When(x => x.AgeRating != null);
        RuleFor(x => x.Ratings)
            .ChildRules(ratings =>
            {
                ratings.RuleFor(r => r!.ImdbRating).LessThanOrEqualTo(10);
                ratings.RuleFor(r => r!.KinopoiskRating).LessThanOrEqualTo(10);
                ratings.RuleFor(r => r!.LocalRating).LessThanOrEqualTo(10);
            });
        RuleFor(x => x.TrailerInfo)
            .ChildRules(trailerInfo =>
            {
                trailerInfo.RuleFor(ti => ti!.Name).NotEmpty().MaximumLength(60);
                trailerInfo.RuleFor(ti => ti!.Url).NotEmpty();
            })
            .When(x => x.TrailerInfo != null);
        RuleFor(x => x.Budget)
            .ChildRules(budget =>
            {
                budget.RuleFor(b => b!.BudgetValue).NotEmpty();
                budget.RuleFor(b => b!.BudgetCurrencyName).NotEmpty().MaximumLength(10);
            })
            .When(x => x.Budget != null);
        RuleFor(x => x.Genres)
            .NotEmpty();
        RuleForEach(x => x.Genres).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PersonsInContent).NotEmpty();
        RuleFor(x => x.AllowedSubscriptions).NotEmpty();
        RuleFor(x => x.AllowedSubscriptions)
            .MustAsync(AreSubscriptionsExistAsync)
            .WithMessage("Нельзя добавить свою подписку");
        RuleForEach(x => x.PersonsInContent).ChildRules(pic =>
        {
            pic.RuleFor(picdto => picdto.Name).NotEmpty().MaximumLength(70);
            pic.RuleFor(picdto => picdto.Profession).NotEmpty().MaximumLength(70);
        });
        RuleForEach(x => x.AllowedSubscriptions).ChildRules(sub =>
        {
            sub.RuleFor(subdto => subdto.Name).NotEmpty().MaximumLength(50);
        });
        RuleFor(x => x.ReleaseYears).ChildRules(ry =>
        {
            ry.RuleFor(releaseYear => releaseYear.End).NotEmpty()
                .GreaterThan(releaseYear => releaseYear.Start);
            ry.RuleFor(releaseYear => releaseYear.End).NotEmpty();
        });
        RuleForEach(x => x.SeasonInfos).ChildRules(si =>
        {
            si.RuleFor(sii => sii.SeasonNumber).NotEmpty();
            si.RuleForEach(sii => sii.Episodes).ChildRules(ep =>
            {
                ep.RuleFor(epi => epi.EpisodeNumber).NotEmpty();
                ep.RuleFor(epi => epi.VideoUrl).NotEmpty();
            });
        });
        RuleFor(x => x.SeasonInfos).NotEmpty();
    }
    
    private async Task<bool> AreSubscriptionsExistAsync(List<SubscriptionDto> subscriptions, CancellationToken cancellationToken)
    {
        var dbSubscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
        foreach (var subscription in subscriptions)
        {
            if (!dbSubscriptions.Any(dbs => dbs.Name.Equals(subscription.Name)))
            {
                return false;
            }
        }
        
        return true;
    }
}