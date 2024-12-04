using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.ChangeRole;

public record ChangeRoleCommand(UserRoleDto UserRoleDto) : ICommand<ChangeRoleDto>;