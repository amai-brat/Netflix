using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;
using Application.Repositories;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Contents.Commands.UpdateSerialContent;

internal class UpdateSerialContentCommandHandler(
    IMapper mapper,
    IContentRepository contentRepository,
    ContentVideoManager contentVideoManager) : ICommandHandler<UpdateSerialContentCommand>
{
    public async Task Handle(UpdateSerialContentCommand request, CancellationToken cancellationToken)
    {
        var serialContent = mapper.Map<SerialContentDto, SerialContent>(request.ContentDto);
        foreach (var season in request.ContentDto.SeasonInfos)
        {
            foreach (var episode in season.Episodes)
            {
                if (episode.VideoFile != null)
                {
                    await contentVideoManager.PutSerialContentVideoAsync(request.ContentDto.Id, episode.Resolution,
                        season.SeasonNumber, episode.EpisodeNumber, episode.VideoFile);
                }
            }
        }
        await contentRepository.UpdateSerialContent(serialContent);
        await contentRepository.SaveChangesAsync();
    }
}