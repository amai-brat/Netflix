using Application.Cqrs.Queries;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Users.Queries.GetReviews;

internal class GetReviewsQueryHandler(
    IReviewRepository reviewRepository,
    IMapper mapper) : IQueryHandler<GetReviewsQuery, GetReviewsDto>
{
    public async Task<GetReviewsDto> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetByReviewSearchDtoAsync(request.SearchDto, Consts.ReviewsPerPage);
        var reviewDtos = mapper.Map<List<UserReviewDto>>(reviews);
        return new GetReviewsDto { ReviewDtos = reviewDtos };
    }
}