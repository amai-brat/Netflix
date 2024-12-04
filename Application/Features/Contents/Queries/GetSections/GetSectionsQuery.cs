using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.GetSections;

public record GetSectionsQuery : IQuery<GetSectionsDto>;