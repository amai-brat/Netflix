using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.SignUpDto)
            .SetValidator(new SignUpDtoValidator(userRepository));
    }
}

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    private readonly IUserRepository _userRepository;

    public SignUpDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Login)
            .Length(4, 25)
            .WithMessage("Длина логина - 4 - 25 символов")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Допустимые символы: латинские буквы, цифры, _");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Given string isn't email");

        RuleFor(x => x.Email)
            .MustAsync(IsEmailUniqueAsync)
            .WithMessage(ErrorMessages.EmailNotUnique);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Пустой пароль")
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")
            .WithMessage(
                "Минимум 8 символов, хотя бы одна заглавная латинская буква, одна строчная латинская буква, одна цифра и спец. символ");
    }

    private async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellation)
    {
        return await _userRepository.IsEmailUniqueAsync(email);
    }
}


