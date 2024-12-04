using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using FluentValidation;

namespace Application.Features.Comments.Commands.AssignComment;

internal class AssignCommentCommandValidator : AbstractValidator<AssignCommentCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;

    public AssignCommentCommandValidator(IUserRepository userRepository, IReviewRepository reviewRepository)
    {
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage(ErrorMessages.CommentMustHaveText);

        RuleFor(x => x.UserId)
            .MustAsync(IsUserExistAsync)
            .WithMessage(ErrorMessages.NotFoundUser);

        RuleFor(x => x.ReviewId)
            .MustAsync(IsReviewExistAsync)
            .WithMessage(ErrorMessages.NotFoundReview);
    }

    private async Task<bool> IsUserExistAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByFilterAsync(x => x.Id == userId);
        return user != null;
    }

    private async Task<bool> IsReviewExistAsync(long reviewId, CancellationToken cancellationToken)
    {
        var comment = await _reviewRepository.GetReviewByFilterAsync(r => r.Id == reviewId);
        return comment != null;
    }
}