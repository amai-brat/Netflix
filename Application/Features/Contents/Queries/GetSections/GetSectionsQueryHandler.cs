using Application.Cqrs.Queries;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Contents.Queries.GetSections;

internal class GetSectionsQueryHandler(
    IContentTypeRepository contentTypeRepository,
    IContentRepository contentRepository,
    IMapper mapper) : IQueryHandler<GetSectionsQuery, GetSectionsDto>
{
    public async Task<GetSectionsDto> Handle(GetSectionsQuery request, CancellationToken cancellationToken)
    {
        var result = new List<SectionDto>();
        
        var contentTypes = await contentTypeRepository.GetContentTypesAsync();
        foreach (var contentType in contentTypes)
        {
            var contents = await contentRepository
                .GetContentsByFilterWithAmountAsync(x => x.ContentTypeId == contentType.Id, 20);
            
            if (contents.Count <= 0) continue;
            
            result.Add(new SectionDto
            {
                Name = contentType.ContentTypeName,
                Contents = mapper.Map<List<SectionContentDto>>(contents)
            });
        }

        return new GetSectionsDto { SectionDtos = result };
    }
}