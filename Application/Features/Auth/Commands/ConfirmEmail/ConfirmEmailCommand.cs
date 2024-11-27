using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.ConfirmEmail;

public record ConfirmEmailCommand(long UserId, string Token) : ICommand<ConfirmEmailDto>;