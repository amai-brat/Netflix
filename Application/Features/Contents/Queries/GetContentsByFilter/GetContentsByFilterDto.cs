using Domain.Entities;

namespace Application.Features.Contents.Queries.GetContentsByFilter;

public class GetContentsByFilterDto
{
    public List<ContentBase> Contents { get; set; } = null!;
}