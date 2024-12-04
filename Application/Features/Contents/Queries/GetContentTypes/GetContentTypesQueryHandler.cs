using Application.Cqrs.Queries;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Contents.Queries.GetContentTypes;

internal class GetContentTypesQueryHandler(
    IContentTypeRepository contentTypeRepository,
    IMapper mapper) : IQueryHandler<GetContentTypesQuery, GetContentTypesDto>
{
    public async Task<GetContentTypesDto> Handle(GetContentTypesQuery request, CancellationToken cancellationToken)
    {
        var contentTypes = await contentTypeRepository.GetContentTypesAsync();
        return new GetContentTypesDto { ContentTypeDtos = mapper.Map<List<ContentTypeDto>>(contentTypes) };
    }
}