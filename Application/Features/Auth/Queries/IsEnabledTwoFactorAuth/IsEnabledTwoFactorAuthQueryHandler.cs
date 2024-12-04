using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Queries.IsEnabledTwoFactorAuth;

internal class IsEnabledTwoFactorAuthQueryHandler(
    UserManager<AppUser> userManager) : IQueryHandler<IsEnabledTwoFactorAuthQuery, IsEnabledTwoFactorAuthDto>
{
    public async Task<IsEnabledTwoFactorAuthDto> Handle(IsEnabledTwoFactorAuthQuery request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByEmailAsync(request.Email);
        if (appUser == null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        var enabled = await userManager.GetTwoFactorEnabledAsync(appUser);
        return new IsEnabledTwoFactorAuthDto { Enabled = enabled };
    }
}