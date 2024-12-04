using Application.Cqrs.Queries;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Contents.Queries.GetPromos;

internal class GetPromosQueryHandler(
    IContentRepository contentRepository,
    IMapper mapper) : IQueryHandler<GetPromosQuery, GetPromosDto>
{
    public async Task<GetPromosDto> Handle(GetPromosQuery request, CancellationToken cancellationToken)
    {
        var contents = await contentRepository.GetRandomContentsAsync(5);
        return new GetPromosDto { PromoDtos = mapper.Map<List<PromoDto>>(contents) };
    }
}