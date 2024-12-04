using System.Net;
using System.Net.Http.Headers;
using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Contents.Queries.Streaming.GetSerialContentStreamChunk;

internal class GetSerialContentStreamChunkQueryHandler(
    IContentVideoManager contentVideoManager,
    IHttpClientFactory clientFactory,
    IPermissionChecker permissionChecker,
    IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetSerialContentStreamChunkQuery, GetSerialContentStreamChunkDto>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    
    public async Task<GetSerialContentStreamChunkDto> Handle(GetSerialContentStreamChunkQuery request, CancellationToken cancellationToken)
    {
        var userCanViewContent = await permissionChecker.IsContentAllowedForUserAsync(request.SerialId, request.UserId);
        if (!userCanViewContent)
        {
            throw new NotPermittedException("Вам нужна подписка, чтобы смотреть этот контент");
        }
        
        // получаем диапазон байтов
        var range = _httpContext.Request.Headers.Range.ToString();
        var start = 0L;
        var end = 0L;
        if (!string.IsNullOrEmpty(range))
        {
            var rangeParts = range.Replace("bytes=", "").Split('-');
            start = long.Parse(rangeParts[0]);
            end = rangeParts.Length > 1 && !string.IsNullOrEmpty(rangeParts[1]) ? long.Parse(rangeParts[1]) : 0;
        }
        // создаем http клиент
        var httpClient = clientFactory.CreateClient();
        var videoStreamUrl = await contentVideoManager.GetSerialContentStreamUrlAsync(
            request.SerialId, request.Season, request.Episode, request.Resolution);
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, videoStreamUrl);
        if (start != 0 || end != 0)
        {
            requestMessage.Headers.Range = new RangeHeaderValue(start, end);
        }
            
        // отправляем запрос на сервер с видео(minio)
        var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new GetSerialContentStreamChunkDto
            {
                ErrorCode = (int)response.StatusCode,
                Error = "Не удалось получить видео"
            };
        }
        var contentLength = response.Content.Headers.ContentLength;
        if (!contentLength.HasValue)
        {
            return new GetSerialContentStreamChunkDto
            {
                ErrorCode = StatusCodes.Status500InternalServerError,
                Error = "Не удалось получить длину контента"
            };
        }
        end = end == 0 ? contentLength.Value - 1 : end;
        // получаем стрим и отдаем его клиенту
        var videoStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        _httpContext.Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{contentLength}");
        _httpContext.Response.Headers.Append("Accept-Ranges", "bytes");
        _httpContext.Response.Headers.Append("Content-Length", (end - start + 1).ToString());
        _httpContext.Response.StatusCode = (int)HttpStatusCode.PartialContent;

        return new GetSerialContentStreamChunkDto
        {
            VideoStream = videoStream
        };
    }
}