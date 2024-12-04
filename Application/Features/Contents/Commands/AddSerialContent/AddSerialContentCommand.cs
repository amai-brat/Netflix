using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;

namespace Application.Features.Contents.Commands.AddSerialContent;

public record AddSerialContentCommand(SerialContentDto ContentDto) : ICommand;