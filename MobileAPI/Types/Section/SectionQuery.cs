using Application.Features.Contents.Queries.GetSections;
using HotChocolate.Language;
using MediatR;

namespace MobileAPI.Types.Section;

[ExtendObjectType(OperationType.Query)]
public class SectionQuery
{
    public async Task<IEnumerable<SectionDto>> Sections([Service] IMediator mediator)
    {
        var result = await mediator.Send(new GetSectionsQuery());
        return result.SectionDtos;
    }
}