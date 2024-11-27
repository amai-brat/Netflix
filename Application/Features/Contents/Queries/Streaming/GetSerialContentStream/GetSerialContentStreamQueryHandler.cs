using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Services.Abstractions;

namespace Application.Features.Contents.Queries.Streaming.GetSerialContentStream;

internal class GetSerialContentStreamQueryHandler(
    IContentVideoManager contentVideoManager,
    IHttpClientFactory clientFactory,
    IPermissionChecker permissionChecker) : IQueryHandler<GetSerialContentStreamQuery, GetSerialContentStreamDto>
{
    public async Task<GetSerialContentStreamDto> Handle(GetSerialContentStreamQuery request, CancellationToken cancellationToken)
    {
        var userCanViewContent = await permissionChecker.IsContentAllowedForUserAsync(request.SerialId, request.UserId);
        if (!userCanViewContent)
        {
            throw new NotPermittedException("Вам нужна подписка, чтобы смотреть этот контент");
        }
        
        var videoStreamUrl = await contentVideoManager.GetSerialContentM3U8UrlAsync(
            request.SerialId, request.Season, request.Episode, request.Resolution);
            
        var resp = await clientFactory
            .CreateClient()
            .GetAsync(videoStreamUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        
        if (!resp.IsSuccessStatusCode)
        {
            return new GetSerialContentStreamDto
            {
                ErrorCode = (int)resp.StatusCode,
                Error = "Не удалось получить видео."
            };
        }
        
        var videoStream = await resp.Content.ReadAsStreamAsync(cancellationToken);
        return new GetSerialContentStreamDto
        {
            VideoStream = videoStream
        };
    }
}