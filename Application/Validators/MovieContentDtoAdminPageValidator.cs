using Application.Dto;
using FluentValidation;

namespace Application.Validators;

public class MovieContentDtoAdminPageValidator : AbstractValidator<MovieContentAdminPageDto>
{
    public MovieContentDtoAdminPageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);
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
        
    }
}