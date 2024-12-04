using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.ChangePassword;

internal class ChangePasswordCommandHandler(
    UserManager<AppUser> userManager) : ICommandHandler<ChangePasswordCommand, ChangePasswordDto>
{
    public async Task<ChangePasswordDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByEmailAsync(request.UserEmail);
        if (appUser is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        var result = await userManager.ChangePasswordAsync(appUser, request.PreviousPassword, request.NewPassword);
        if (result.Succeeded)
        {
            return new ChangePasswordDto { Email = appUser.Email! };
        }

        throw new IdentityException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }
}