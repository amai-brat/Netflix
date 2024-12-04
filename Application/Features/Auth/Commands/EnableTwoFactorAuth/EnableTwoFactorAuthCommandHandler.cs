using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.EnableTwoFactorAuth;

internal class EnableTwoFactorAuthCommandHandler(
    UserManager<AppUser> userManager) : ICommandHandler<EnableTwoFactorAuthCommand>
{
    public async Task Handle(EnableTwoFactorAuthCommand request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByEmailAsync(request.Email);
        if (appUser == null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        await userManager.SetTwoFactorEnabledAsync(appUser, true);
    }
}