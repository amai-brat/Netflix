using Application.Features.Contents.Commands.AddSerialContent;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Contents.Commands.UpdateSerialContent;

internal class UpdateSerialContentCommandValidator : AbstractValidator<UpdateSerialContentCommand>
{
    public UpdateSerialContentCommandValidator(ISubscriptionRepository subscriptionRepository)
    {
        RuleFor(x => x.ContentDto)
            .SetValidator(new SerialContentDtoValidator(subscriptionRepository));
    }
}