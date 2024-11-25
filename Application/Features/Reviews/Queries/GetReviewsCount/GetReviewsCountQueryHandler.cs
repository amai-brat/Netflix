using Application.Cqrs.Queries;
using Application.Repositories;

namespace Application.Features.Reviews.Queries.GetReviewsCount;

internal class GetReviewsCountQueryHandler(
    IReviewRepository reviewRepository) : IQueryHandler<GetReviewsCountQuery, GetReviewsCountDto>
{
    public async Task<GetReviewsCountDto> Handle(GetReviewsCountQuery request, CancellationToken cancellationToken)
    {
        var count = await reviewRepository.GetReviewsCountAsync(request.ContentId);
        return new GetReviewsCountDto { Count = count };
    }
}