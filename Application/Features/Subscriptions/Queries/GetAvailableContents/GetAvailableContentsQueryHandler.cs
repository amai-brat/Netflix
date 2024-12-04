using Application.Cqrs.Queries;
using Application.Repositories;

namespace Application.Features.Subscriptions.Queries.GetAvailableContents;

internal class GetAvailableContentsQueryHandler(
    IContentRepository contentRepository) : IQueryHandler<GetAvailableContentsQuery, GetAvailableContentsDto>
{
    public async Task<GetAvailableContentsDto> Handle(GetAvailableContentsQuery request, CancellationToken cancellationToken)
    {
        var contents = await contentRepository.GetContentsByFilterAsync(x => true);
        var dtos = contents
            .Select(x => new SubscriptionContentDto { Id = x.Id, Name = x.Name })
            .ToList();

        return new GetAvailableContentsDto { Dtos = dtos };
    }
}