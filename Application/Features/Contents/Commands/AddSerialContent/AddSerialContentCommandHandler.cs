using Application.Cqrs.Commands;
using Application.Features.Contents.Dtos;
using Application.Repositories;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Contents.Commands.AddSerialContent;

internal class AddSerialContentCommandHandler(
    IMapper mapper,
    IContentRepository contentRepository,
    ContentVideoManager contentVideoManager) : ICommandHandler<AddSerialContentCommand>
{
    public async Task Handle(AddSerialContentCommand request, CancellationToken cancellationToken)
    {
        var serialContent = mapper.Map<SerialContentDto, SerialContent>(request.ContentDto);
        contentRepository.AddSerialContent(serialContent);
        await contentRepository.SaveChangesAsync();
        foreach (var season in request.ContentDto.SeasonInfos)
        {
            foreach (var episode in season.Episodes)
            {
                if (episode.VideoFile != null)
                {
                    await contentVideoManager.PutSerialContentVideoAsync(serialContent.Id, episode.Resolution,
                        season.SeasonNumber, episode.EpisodeNumber, episode.VideoFile);
                }
            }
        }
    }
}