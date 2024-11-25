using Application.Cqrs.Queries;
using Application.Dto;

namespace Application.Features.Users.Queries.GetReviewsPagesCount;

public record GetReviewsPagesCountQuery(ReviewSearchDto SearchDto) : IQuery<GetReviewsPageCountDto>;