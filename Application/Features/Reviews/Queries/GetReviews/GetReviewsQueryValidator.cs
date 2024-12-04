using Application.Exceptions.ErrorMessages;
using FluentValidation;

namespace Application.Features.Reviews.Queries.GetReviews;

internal class GetReviewsQueryValidator : AbstractValidator<GetReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ErrorMessages.ArgumentsMustBePositive + ": Limit");
        
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ErrorMessages.ArgumentsMustBePositive + ": Offset");
    }
}