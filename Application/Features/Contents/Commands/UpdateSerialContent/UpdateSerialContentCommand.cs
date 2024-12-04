using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;

namespace Application.Features.Contents.Commands.UpdateSerialContent;

public record UpdateSerialContentCommand(SerialContentDto ContentDto) : ICommand;