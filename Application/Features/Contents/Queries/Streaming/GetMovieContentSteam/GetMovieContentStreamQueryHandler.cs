using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Services.Abstractions;

namespace Application.Features.Contents.Queries.Streaming.GetMovieContentSteam;

internal class GetMovieContentStreamQueryHandler(
    IContentVideoManager contentVideoManager,
    IHttpClientFactory clientFactory,
    IPermissionChecker permissionChecker) : IQueryHandler<GetMovieContentStreamQuery, GetMovieContentStreamDto>
{
    public async Task<GetMovieContentStreamDto> Handle(GetMovieContentStreamQuery request, CancellationToken cancellationToken)
    {
        var userCanViewContent = await permissionChecker.IsContentAllowedForUserAsync(request.MovieId, request.UserId);
        if (!userCanViewContent)
        {
            throw new NotPermittedException("Вам нужна подписка, чтобы смотреть этот контент");
        }
        
        var videoStreamUrl = await contentVideoManager.GetMovieContentM3U8UrlAsync(request.MovieId, request.Resolution);
        var resp = await clientFactory
            .CreateClient()
            .GetAsync(videoStreamUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        
        if (!resp.IsSuccessStatusCode)
        {
            return new GetMovieContentStreamDto
            {
                ErrorCode = (int)resp.StatusCode,
                Error = "Не удалось получить видео."
            };
        }
        
        var videoStream = await resp.Content.ReadAsStreamAsync(cancellationToken);
        return new GetMovieContentStreamDto
        {
            VideoStream = videoStream
        };
    }
}