using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Favourites.Commands.RemoveFavourite;

public class RemoveFavouriteCommandValidator : AbstractValidator<RemoveFavouriteCommand>
{
    private readonly IContentRepository _contentRepository;
    private readonly IFavouriteContentRepository _favouriteContentRepository;
    private readonly IUserRepository _userRepository;

    public RemoveFavouriteCommandValidator(
        IContentRepository contentRepository,
        IFavouriteContentRepository favouriteContentRepository,
        IUserRepository userRepository)
    {
        _contentRepository = contentRepository;
        _favouriteContentRepository = favouriteContentRepository;
        _userRepository = userRepository;

        RuleFor(x => x.UserId)
            .MustAsync(IsUserExistAsync)
            .WithMessage(ErrorMessages.NotFoundUser);

        RuleFor(x => x.ContentId)
            .MustAsync(IsContentExistAsync)
            .WithMessage(ErrorMessages.NotFoundContent);

        RuleFor(x => x)
            .MustAsync(async (command, ct) => 
                await IsFavouriteAsync(command.ContentId, command.UserId, ct))
            .WithMessage(ErrorMessages.NotInFavourite);
    }

    private async Task<bool> IsUserExistAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByFilterAsync(u => u.Id == userId);
        return user != null;
    }

    private async Task<bool> IsContentExistAsync(long contentId, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetContentByFilterAsync(c => c.Id == contentId);
        return content != null;
    }

    private async Task<bool> IsFavouriteAsync(long contentId, long userId, CancellationToken _)
    {
        var list = await _favouriteContentRepository
            .GetFavouriteContentsByFilterAsync(f =>
                f.UserId == userId &&
                f.ContentId == contentId);

        return list.Count != 0;
    }
}