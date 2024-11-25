using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Users.Commands.ChangeBirthday;

internal class ChangeBirthdayCommandValidator : AbstractValidator<ChangeBirthdayCommand>
{
    private readonly IUserRepository _userRepository;

    public ChangeBirthdayCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserId)
            .MustAsync(IsUserExistAsync)
            .WithMessage(ErrorMessages.NotFoundUser);

        RuleFor(x => x.NewBirthday)
            .Must(x =>
                x < DateOnly.FromDateTime(DateTime.UtcNow) &&
                x > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-150)))
            .WithMessage(ErrorMessages.InvalidBirthday);
    }

    private async Task<bool> IsUserExistAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByFilterAsync(x => x.Id == userId);
        return user != null;
    }
}