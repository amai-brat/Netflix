using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Reviews.Commands.AssignReview;

internal class AssignReviewCommandValidator : AbstractValidator<AssignReviewCommand>
{
    private readonly IContentRepository _contentRepository;
    private readonly IUserRepository _userRepository;

    public AssignReviewCommandValidator(
        IContentRepository contentRepository, 
        IUserRepository userRepository)
    {
        _contentRepository = contentRepository;
        _userRepository = userRepository;

        RuleFor(x => x.AssignDto.Text)
            .NotEmpty()
            .WithMessage(ErrorMessages.ReviewMustHaveText);
        
        RuleFor(x => x.AssignDto.Score)
            .Must(x => x is >= 0 and <= 10)
            .When(x => x.AssignDto.Score is not null)
            .WithMessage(ErrorMessages.ScoreMustBeValid);
        
        RuleFor(x => x.AssignDto.ContentId)
            .MustAsync(IsContentExistAsync)
            .WithMessage(ErrorMessages.NotFoundContent);
        
        RuleFor(x => x.UserId)
            .MustAsync(IsUserExistAsync)
            .WithMessage(ErrorMessages.NotFoundUser);
    }

    private async Task<bool> IsContentExistAsync(long contentId, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetContentByFilterAsync(x => x.Id == contentId);
        return content != null;
    }

    private async Task<bool> IsUserExistAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByFilterAsync(u => u.Id == userId);
        return user != null;
    }
}