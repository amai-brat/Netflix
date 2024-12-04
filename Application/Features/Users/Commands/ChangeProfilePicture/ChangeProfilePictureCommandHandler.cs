using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Providers;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Users.Commands.ChangeProfilePicture;

internal class ChangeProfilePictureCommandHandler(
    IUserRepository userRepository,
    IProfilePicturesProvider profilePicturesProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeProfilePictureCommand, User>
{
    public async Task<User> Handle(ChangeProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == request.UserId);
        if (user is null)
        {
            throw new ArgumentValidationException(ErrorMessages.NotFoundUser);
        }

        var pictureName = Guid.NewGuid().ToString();
        await profilePicturesProvider.PutAsync(pictureName, request.PictureStream, request.ContentType);
        
        user.ProfilePictureUrl = pictureName;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}