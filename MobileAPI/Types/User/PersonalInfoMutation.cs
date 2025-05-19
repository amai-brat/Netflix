using Application.Features.Auth.Commands.ChangeEmailRequest;
using Application.Features.Auth.Commands.ChangePassword;
using Application.Features.Users.Commands.ChangeBirthday;
using Application.Features.Users.Queries.GetPersonalInfo;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MediatR;
using MobileAPI.Helpers;
using System.Security.Claims;

namespace MobileAPI.Types.User;

[ExtendObjectType(OperationType.Mutation)]
public class PersonalInfoMutation
{
    [Authorize]
    public async Task<PersonalInfoDto> ChangeEmail(
        [Argument] string newEmail,
        [Service] IHttpContextAccessor accessor,
        [Service] IMediator mediator)
    {
        var userId = accessor.HttpContext!.GetUserId();
        var userEmail = accessor.HttpContext!.User.FindFirst(ClaimTypes.Email)!.Value;
        await mediator.Send(new ChangeEmailRequestCommand(userEmail, newEmail));
        var infoDto = await mediator.Send(new GetPersonalInfoQuery(userId));

        return infoDto;
    }

    [Authorize]
    public async Task<PersonalInfoDto> ChangeBirthDay(
        [Argument] DateOnly birthDay,
        [Service] IHttpContextAccessor accessor,
        [Service] IMediator mediator)
    {
        var userId = accessor.HttpContext!.GetUserId();
        _ = await mediator.Send(new ChangeBirthdayCommand(userId, birthDay));
        var infoDto = await mediator.Send(new GetPersonalInfoQuery(userId));

        return infoDto;
    }

    [Authorize]
    public async Task<ChangePasswordDto> ChangePassword(
        PasswordsDto passwords,
        [Service] IHttpContextAccessor accessor,
        [Service] IMediator mediator)
    {
        var userEmail = accessor.HttpContext!.User.FindFirst(ClaimTypes.Email)!.Value;
        var result = await mediator.Send(new ChangePasswordCommand(userEmail, passwords.PreviousPassword, passwords.NewPassword));

        return result;
    }
}
