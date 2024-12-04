using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.ConfirmEmail;

internal class ConfirmEmailCommandHandler(
    UserManager<AppUser> userManager) : ICommandHandler<ConfirmEmailCommand, ConfirmEmailDto>
{
    public async Task<ConfirmEmailDto> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded)
        {
            return new ConfirmEmailDto { Email = user.Email! };
        }

        throw new IdentityException(AuthErrorMessages.InvalidConfirmationToken);
    }
}