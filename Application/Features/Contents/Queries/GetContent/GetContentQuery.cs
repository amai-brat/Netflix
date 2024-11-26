using Application.Cqrs.Queries;

namespace Application.Features.Contents.Queries.GetContent;

public record GetContentQuery() : IQuery<object>;