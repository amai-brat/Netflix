using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using Domain.Entities;
using MassTransit;
using Shared.MessageContracts;

namespace Application.Features.Contents.Queries.GetContent;

// чтобы ничего не сломать в фронте, просто скопировал, что есть в контроллере, а так это бред как будто
internal class GetContentQueryHandler(
    IBus bus,
    IContentRepository contentRepository) : IQueryHandler<GetContentQuery, object>
{
    private readonly JsonSerializerOptions _options = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public async Task<object> Handle(GetContentQuery request, CancellationToken cancellationToken)
    {
        var content = await contentRepository.GetContentByFilterAsync(x => x.Id == request.ContentId);
        if (content != null)
        {
            await bus.Publish(new ContentPageOpenedEvent(request.ContentId), cancellationToken);
        }
        
        switch (content)
        {
            case null:
                throw new ArgumentValidationException(ErrorMessages.NotFoundContent);
            case SerialContent:
            {
                var serialContent = await contentRepository.GetSerialContentByFilterAsync(x => x.Id == request.ContentId);
                serialContent = SetConstraintOnPersonCount(serialContent!);
                var serializedSerialContent = JsonSerializer.Serialize(serialContent, _options);
                return serializedSerialContent;
            }
            case MovieContent:
                var movie = await contentRepository.GetMovieContentByFilterAsync(x => x.Id == request.ContentId);
                return SetConstraintOnPersonCount(movie!);
            default:
                return SetConstraintOnPersonCount(content);
        }
    }
    
    private T SetConstraintOnPersonCount<T>(T content) where T : ContentBase
    {
        content.PersonsInContent = content.PersonsInContent.GroupBy(p => p.ProfessionId)
            .SelectMany(p => p.Take(Consts.MaxReturnPersonPerRole))
            .ToList();
        SetUpContent(content);
        return content;
    }

    private void SetUpContent(ContentBase content)
    {
        content.Genres.ForEach(g => g.Contents = null!);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if(content.ContentType != null)
            content.ContentType.ContentsWithType = null;
        content.PersonsInContent.ForEach(p => p.Content = null!);
        content.AllowedSubscriptions.ForEach(a => a.AccessibleContent = null!);
    }
}