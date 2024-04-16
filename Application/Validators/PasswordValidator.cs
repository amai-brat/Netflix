using FluentValidation;

namespace Infrastucture.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Пустой пароль")
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")
            .WithMessage(
                "Минимум 8 символов, хотя бы одна заглавная латинская буква, одна строчная латинская буква, одна цифра и спец. символ");
    }
}