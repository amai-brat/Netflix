using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.ChangeEmail;

public record ChangeEmailCommand(long UserId, string NewEmail, string Token) : ICommand;