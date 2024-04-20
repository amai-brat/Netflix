using Application.Dto;
using FluentValidation;

namespace Infrastructure.Validators;

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(x => x.Login)
            .Length(4, 25)
            .WithMessage("Длина логина - 4 - 25 символов")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Допустимые символы: латинские буквы, цифры, _");
        
        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());
    }
}