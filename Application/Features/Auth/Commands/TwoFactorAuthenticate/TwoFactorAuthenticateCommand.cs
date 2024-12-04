using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.TwoFactorAuthenticate;

public record TwoFactorAuthenticateCommand(TwoFactorTokenDto TwoFactorTokenDto) : ICommand<TwoFactorAuthenticateDto>;