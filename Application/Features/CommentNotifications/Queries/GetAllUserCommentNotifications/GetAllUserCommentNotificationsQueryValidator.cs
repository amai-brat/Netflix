using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.CommentNotifications.Queries.GetAllUserCommentNotifications;

internal class GetAllUserCommentNotificationsQueryValidator : AbstractValidator<GetAllUserCommentNotificationsQuery>
{
    private readonly IUserRepository _userRepository;

    public GetAllUserCommentNotificationsQueryValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserId)
            .MustAsync(IsUserExistAsync)
            .WithMessage(ErrorMessages.NotFoundUser);
    }

    private async Task<bool> IsUserExistAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByFilterAsync(u => u.Id == userId);
        return user != null;
    }
}