using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.EnableTwoFactorAuth;

public record EnableTwoFactorAuthCommand(string Email) : ICommand;