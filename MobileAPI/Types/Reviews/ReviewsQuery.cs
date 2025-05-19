using Application.Dto;
using Application.Features.Users.Queries.GetReviewsPagesCount;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MediatR;
using HotChocolate.Data;
using MobileAPI.Helpers;
using UserReviewDto = Application.Features.Users.Queries.GetReviews.UserReviewDto;

namespace MobileAPI.Types.Reviews;

[ExtendObjectType(OperationType.Query)]
public class ReviewsQuery
{
    [Authorize]
    public async Task<int> GetReviewsPagesCount(
        [Argument] string search,
        [Service] IHttpContextAccessor accessor,
        [Service] IMediator mediator)
    {
        var userId = accessor.HttpContext!.GetUserId();
        var dto = new ReviewSearchDto
        {
            UserId = userId,
            Search = search
        };

        var result = await mediator.Send(new GetReviewsPagesCountQuery(dto));

        return result.Count;
    }

    [Authorize]
    [UseOffsetPaging]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IQueryable<UserReviewDto> GetReviews(
        [Service] AppDbContext dbContext,
        [Service] IMapper mapper,
        [Service] IHttpContextAccessor accessor,
        [Service] IMediator mediator)
    {
        var userId = accessor.HttpContext!.GetUserId();

        return dbContext.Reviews
            .Where(r => r.UserId == userId)
            .ProjectTo<UserReviewDto>(mapper.ConfigurationProvider);
    }
}
