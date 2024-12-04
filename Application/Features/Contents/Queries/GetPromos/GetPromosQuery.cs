using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.GetPromos;

public record GetPromosQuery : IQuery<GetPromosDto>;