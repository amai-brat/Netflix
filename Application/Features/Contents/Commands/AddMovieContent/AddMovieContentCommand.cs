using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;

namespace Application.Features.Contents.Commands.AddMovieContent;

public record AddMovieContentCommand(MovieContentDto ContentDto) : ICommand;