using Application.Cqrs.Queries;
using Application.Dto;

namespace Application.Features.Users.Queries.GetReviews;

public record GetReviewsQuery(ReviewSearchDto SearchDto) : IQuery<GetReviewsDto>;
