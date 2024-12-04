using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.GetContentsByFilter;

public record GetContentsByFilterQuery(Filter Filter) : IQuery<GetContentsByFilterDto>;