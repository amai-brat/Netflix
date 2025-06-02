using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SupportAPI.Filters;
using SupportAPI.Helpers;
using SupportAPI.Options;
using SupportAPI.Services.Abstractions;
using ContentDispositionHeaderValue = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace SupportAPI.Controller;

[ApiController]
public class FileUploadController(
    IFileUploadService fileUploadService,
    IOptions<MinioProxyOptions> proxyOptions): ControllerBase
{
    
    [HttpPost("support/chats/{id}/files/upload"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
    [DisableFormValueModelBinding]
    [Authorize]
    public async Task<IActionResult> SendMessageWithFile(int id, CancellationToken cancellationToken)
    {
        Stream? currentStream;
        if (!MultipartRequestHelper.IsMultipartContentType(HttpContext.Request.ContentType))
        {
            return BadRequest("Request must be multipart/form-data");
        }
        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 70);
        var reader = new MultipartReader(boundary, HttpContext.Request.Body);
        var section = await reader.ReadNextSectionAsync(cancellationToken);
        var urls = new List<string>();
        while (section != null)
        {
            if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition) &&
                MediaTypeHeaderValue.TryParse(section.ContentType, out var contentType))
            {
                if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                {
                    currentStream = section.Body;
                    var metadata = new Dictionary<string, string>
                    {
                        { "role", User.Claims.First(c => c.Type == ClaimTypes.Role).Value },
                        { "for", id.ToString() }
                    };
                    var result = await fileUploadService.UploadFileAndSaveMetadataAsync(
                        currentStream, 
                        contentType.MediaType.ToString(), 
                        metadata, 
                        cancellationToken);
                    await currentStream.DisposeAsync();

                    await fileUploadService.ExtractFileMetadataToTempDatabaseAsync(result.Name, cancellationToken);
                    urls.Add(result.Url);
                }
                else
                {
                    return BadRequest("Invalid file format");
                }
            }
            section = await reader.ReadNextSectionAsync(cancellationToken);
        }
        
        var proxiedUris = GetProxiedUris(urls.Select(x => new Uri(x)));
        return Ok(proxiedUris);
    }

    private List<Uri> GetProxiedUris(IEnumerable<Uri> uris)
    {
        var result = new List<Uri>();
        var options = proxyOptions.Value;
        foreach (var uri in uris)
        {
            var uriRewrite = new UriBuilder(uri)
            {
                Scheme = options.Scheme,
                Host = options.Host,
                Port = options.Port,
                Path = options.RoutingKey + uri.AbsolutePath
            }.Uri;
            result.Add(uriRewrite);
        }

        return result;
    }
}