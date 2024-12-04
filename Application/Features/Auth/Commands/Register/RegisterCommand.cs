using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.Register;

public record RegisterCommand(SignUpDto SignUpDto) : ICommand<RegisterDto>;