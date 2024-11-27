using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand : ICommand<RefreshTokenDto>;