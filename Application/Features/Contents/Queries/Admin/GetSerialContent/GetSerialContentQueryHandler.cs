using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Features.Contents.Dtos;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Contents.Queries.Admin.GetSerialContent;

internal class GetSerialContentQueryHandler(
    IMapper mapper,
    IContentRepository contentRepository) : IQueryHandler<GetSerialContentQuery, SerialContentDto>
{
    public async Task<SerialContentDto> Handle(GetSerialContentQuery request, CancellationToken cancellationToken)
    {
        var serialContent = await contentRepository.GetSerialContentByFilterAsync(x => x.Id == request.SerialId);
        if (serialContent is null)
        {
            throw new ArgumentValidationException("такого контента нет");
        }

        var dto = mapper.Map<SerialContentDto>(serialContent);
        return dto;
    }
}