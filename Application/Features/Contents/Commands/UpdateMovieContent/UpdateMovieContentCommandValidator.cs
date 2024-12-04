using Application.Features.Contents.Commands.AddMovieContent;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Contents.Commands.UpdateMovieContent;

internal class UpdateMovieContentCommandValidator : AbstractValidator<UpdateMovieContentCommand>
{
    public UpdateMovieContentCommandValidator(ISubscriptionRepository subscriptionRepository)
    {
        RuleFor(x => x.ContentDto)
            .SetValidator(new MovieContentDtoValidator(subscriptionRepository));
    }
}