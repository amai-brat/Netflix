using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.RevokeToken;

public record RevokeTokenCommand : ICommand<RevokeTokenDto>;