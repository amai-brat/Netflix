using Application.Cqrs.Queries;
using Application.Features.Contents.Dtos;

namespace Application.Features.Contents.Queries.Admin.GetSerialContent;

public record GetSerialContentQuery(long SerialId) : IQuery<SerialContentDto>;