using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Contents.Commands.AddMovieContent;

internal class AddMovieContentCommandHandler(
    IMapper mapper,
    IContentRepository contentRepository,
    IContentVideoManager contentVideoManager) : ICommandHandler<AddMovieContentCommand>
{
    public async Task Handle(AddMovieContentCommand request, CancellationToken cancellationToken)
    {
        var movieContent = mapper.Map<MovieContentDto, MovieContent>(request.ContentDto);
        contentRepository.AddMovieContent(movieContent);
        await contentRepository.SaveChangesAsync();
        if (request.ContentDto.VideoFile != null)
        {
            await contentVideoManager.PutMovieContentVideoAsync(movieContent.Id, request.ContentDto.Resolution, request.ContentDto.VideoFile);
        }
    }
}