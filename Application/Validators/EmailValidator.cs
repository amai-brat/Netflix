using FluentValidation;

namespace Application.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .EmailAddress()
            .WithMessage("Given string isn't email");
    }
}