using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Providers;
using Application.Repositories;

namespace Application.Features.Users.Queries.GetPersonalInfo;

internal class GetPersonalInfoQueryHandler(
    IUserRepository userRepository,
    IProfilePicturesProvider profilePicturesProvider) : IQueryHandler<GetPersonalInfoQuery, PersonalInfoDto>
{
    public async Task<PersonalInfoDto> Handle(GetPersonalInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == request.UserId);
        if (user is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        string? pictureUrl = null;
        if (user.ProfilePictureUrl != null)
        {
            pictureUrl = await profilePicturesProvider.GetUrlAsync(user.ProfilePictureUrl);
        }

        return new PersonalInfoDto
        {
            Nickname = user.Nickname,
            ProfilePictureUrl = pictureUrl,
            Email = user.Email,
            BirthDay = user.BirthDay?.ToString("dd.MM.yyyy")
        };
    }
}