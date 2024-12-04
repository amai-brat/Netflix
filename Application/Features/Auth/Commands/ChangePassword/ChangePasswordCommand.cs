using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(string UserEmail, string PreviousPassword, string NewPassword) : ICommand<ChangePasswordDto>;