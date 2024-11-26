using Application.Dto;
using FluentValidation;

namespace Infrastructure.Validators;

[Obsolete("CQRS")]
public class MovieContentDtoAdminPageValidator : AbstractValidator<MovieContentAdminPageDto>
{
    public MovieContentDtoAdminPageValidator()
    {
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
        RuleFor(x => x.MovieLength)
            .NotEmpty();
        RuleFor(x => x.ReleaseDate)
            .NotEmpty();
        RuleFor(x => x.VideoUrl)
            .NotEmpty();
        RuleFor(x => x.Genres)
            .NotEmpty();
        RuleFor(x => x.PersonsInContent)
            .NotEmpty();
        RuleFor(x => x.AllowedSubscriptions)
            .NotEmpty();
        RuleFor(x => x.AgeRatings)
            .ChildRules(ageRating =>
            {
                ageRating.RuleFor(ar => ar!.Age).LessThanOrEqualTo(21).NotEmpty();
                ageRating.RuleFor(ar => ar!.AgeMpaa).Length(0, 7); 
            }).When(x => x.AgeRatings != null);
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
        RuleForEach(x => x.Genres).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PersonsInContent).NotEmpty();
        RuleFor(x => x.AllowedSubscriptions).NotEmpty();
        RuleForEach(x => x.PersonsInContent).ChildRules(pic =>
        {
            pic.RuleFor(picdto => picdto.Name).NotEmpty().MaximumLength(70);
            pic.RuleFor(picdto => picdto.Profession).NotEmpty().MaximumLength(70);
        });
        RuleForEach(x => x.AllowedSubscriptions).ChildRules(sub =>
        {
            sub.RuleFor(subdto => subdto.Name).NotEmpty().MaximumLength(50);
        });
    }
}