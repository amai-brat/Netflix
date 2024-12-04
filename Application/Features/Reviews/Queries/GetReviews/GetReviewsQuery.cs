using Application.Cqrs.Queries;

namespace Application.Features.Reviews.Queries.GetReviews;

public record GetReviewsQuery(long ContentId, string Sort, int Offset, int Limit) : IQuery<GetReviewsDto>;