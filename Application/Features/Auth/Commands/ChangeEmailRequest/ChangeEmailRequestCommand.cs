using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.ChangeEmailRequest;

public record ChangeEmailRequestCommand(string PrevEmail, string NewEmail) : ICommand;