using Application.Cqrs.Queries;
using Application.Repositories;

namespace Application.Features.Users.Queries.GetReviewsPagesCount;

internal class GetReviewsPagesCountQueryHandler(
    IReviewRepository reviewRepository) : IQueryHandler<GetReviewsPagesCountQuery, GetReviewsPageCountDto>
{
    public async Task<GetReviewsPageCountDto> Handle(GetReviewsPagesCountQuery request, CancellationToken cancellationToken)
    {
        var count = await reviewRepository.GetPagesCountAsync(request.SearchDto, Consts.ReviewsPerPage);
        return new GetReviewsPageCountDto { Count = count };
    }
}