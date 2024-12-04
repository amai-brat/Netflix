using Application.Cqrs.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.Features.Users.Commands.ChangeBirthday;

internal class ChangeBirthdayCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeBirthdayCommand, User>
{
    public async Task<User> Handle(ChangeBirthdayCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == request.UserId);
        user!.BirthDay = request.NewBirthday;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user;
    }
}