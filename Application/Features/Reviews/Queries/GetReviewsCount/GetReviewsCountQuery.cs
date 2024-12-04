using Application.Cqrs.Queries;

namespace Application.Features.Reviews.Queries.GetReviewsCount;

public record GetReviewsCountQuery(long ContentId) : IQuery<GetReviewsCountDto>;