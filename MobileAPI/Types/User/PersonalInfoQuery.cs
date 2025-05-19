using Application.Features.Users.Queries.GetPersonalInfo;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MediatR;
using MobileAPI.Helpers;

namespace MobileAPI.Types.User;

[ExtendObjectType(OperationType.Query)]
public class PersonalInfoQuery
{
    [Authorize]
    public async Task<PersonalInfoDto> PersonalInfo(
        [Service] IHttpContextAccessor accessor,
        [Service] IMediator mediator)
    {
        var userId = accessor.HttpContext!.GetUserId();
        var personalInfo = await mediator.Send(new GetPersonalInfoQuery(userId));
        return personalInfo;
    }
}
