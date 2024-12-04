using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.GetContentTypes;

public record GetContentTypesQuery : IQuery<GetContentTypesDto>;