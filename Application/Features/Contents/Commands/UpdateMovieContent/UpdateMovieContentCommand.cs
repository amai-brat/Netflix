using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;

namespace Application.Features.Contents.Commands.UpdateMovieContent;

public record UpdateMovieContentCommand(MovieContentDto ContentDto) : ICommand;