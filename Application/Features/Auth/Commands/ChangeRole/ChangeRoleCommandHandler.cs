using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Identity;
using Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.ChangeRole;

internal class ChangeRoleCommandHandler(
    IUserRepository userRepository,
    UserManager<AppUser> userManager) : ICommandHandler<ChangeRoleCommand, ChangeRoleDto>
{
    public async Task<ChangeRoleDto> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == request.UserRoleDto.UserId);
        if (user is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }
        
        var appUser = await userManager.FindByEmailAsync(user.Email);
        var result = await userManager.AddToRoleAsync(appUser!, request.UserRoleDto.Role);
        
        if (result.Succeeded)
        {
            return new ChangeRoleDto { Email = appUser!.Email! };
        }

        throw new IdentityException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }
}