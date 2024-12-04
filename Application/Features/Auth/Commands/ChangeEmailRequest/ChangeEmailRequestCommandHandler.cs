using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Helpers;
using Application.Identity;
using Application.Repositories;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.ChangeEmailRequest;

internal class ChangeEmailRequestCommandHandler(
    UserManager<AppUser> userManager,
    IUserRepository userRepository,
    IEmailSender emailSender) : ICommandHandler<ChangeEmailRequestCommand>
{
    public async Task Handle(ChangeEmailRequestCommand request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByEmailAsync(request.PrevEmail);
        var user = await userRepository.GetUserByFilterAsync(x => x.Email == request.PrevEmail);
        
        if (user is null && appUser is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        if (user is null || appUser is null)
        {
            throw new BusinessException(ErrorMessages.NotFoundUser);
        }

        var token = await userManager.GenerateChangeEmailTokenAsync(appUser, request.NewEmail);
        var message = EmailMessageHelper.GetEmailChangeConfirmationMessage(token, appUser.Id, request.NewEmail);
        await emailSender.SendEmailAsync(request.NewEmail, message);
    }
}