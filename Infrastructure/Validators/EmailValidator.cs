using FluentValidation;

namespace Infrastructure.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .EmailAddress()
            .WithMessage("Given string isn't email");
    }
}