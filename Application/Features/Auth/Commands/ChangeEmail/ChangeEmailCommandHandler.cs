using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.ChangeEmail;

internal class ChangeEmailCommandHandler(
    UserManager<AppUser> userManager,
    IUserRepository userRepository,
    IIdentityUnitOfWork unitOfWork) : ICommandHandler<ChangeEmailCommand>
{
    public async Task Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByIdAsync(request.UserId.ToString());
        if (appUser is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }
        var prevEmail = appUser.Email;
        
        var result = await userManager.ChangeEmailAsync(appUser, request.NewEmail, request.Token);
        if (result.Succeeded)
        {
            var user = await userRepository.GetUserByFilterAsync(x => x.Email == prevEmail);
            user!.Email = request.NewEmail;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        throw new IdentityException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }
}