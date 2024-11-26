using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;
using Application.Repositories;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Contents.Commands.UpdateMovieContent;

internal class UpdateMovieContentCommandHandler(
    IMapper mapper,
    IContentRepository contentRepository,
    ContentVideoManager contentVideoManager) : ICommandHandler<UpdateMovieContentCommand>
{
    public async Task Handle(UpdateMovieContentCommand request, CancellationToken cancellationToken)
    {
        if (request.ContentDto.VideoFile != null)
        {
            await contentVideoManager.PutMovieContentVideoAsync(request.ContentDto.Id, request.ContentDto.Resolution, request.ContentDto.VideoFile);
        }
        var movieContent = mapper.Map<MovieContentDto, MovieContent>(request.ContentDto);
        
        await contentRepository.UpdateMovieContent(movieContent);
        await contentRepository.SaveChangesAsync();
    }
}