using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SupportAPI.Filters;
using SupportAPI.Helpers;
using SupportAPI.Options;
using SupportAPI.Services;
using ContentDispositionHeaderValue = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace SupportAPI.Controller;

[ApiController]
public class FileUploadController(
    IFileUploadService fileUploadService,
    IHttpClientFactory clientFactory,
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
            return BadRequest("");
        }
        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(Request.ContentType), 70);
        var reader = new MultipartReader(boundary, HttpContext.Request.Body);
        var section = await reader.ReadNextSectionAsync(cancellationToken);
        var urls = new List<string>();
        while (section != null)
        {
            var hasContentDispositionHeader = 
                ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition);

            if (hasContentDispositionHeader)
            {
                if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                {
                    currentStream = section.Body;
                    var metadata = new Dictionary<string, string>();
                    metadata.Add("role", User.Claims.First(c => c.Type == ClaimTypes.Role).Value);
                    metadata.Add("for", id.ToString());
                    var result = await fileUploadService.UploadFileAndSaveMetadataAsync(currentStream, metadata, cancellationToken);
                    await currentStream.DisposeAsync();
                    urls.Add(result);
                }
                else
                {
                    return BadRequest("Invalid file format");
                }
            }
            section = await reader.ReadNextSectionAsync(cancellationToken);
        }
        
        var client = clientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1]);
        List<Uri>? resp;
        try
        {
            var result = await client.PostAsJsonAsync("http://support-permanent-s3-service:8080/copy-to-perm",
                urls, cancellationToken);
            resp = await result.Content.ReadFromJsonAsync<List<Uri>>(cancellationToken: cancellationToken);
        }
        finally
        {
            await fileUploadService.DeleteFileAndMetadataAsync(urls, cancellationToken);
        }

        var rewrittenUrls = new List<Uri>();
        var options = proxyOptions.Value;
        if (resp != null)
        {
            foreach (var uri in resp)
            {
                var uriRewrite = new UriBuilder(uri)
                {
                    Scheme = options.Scheme,
                    Host = options.Host,
                    Port = options.Port,
                    Path = options.RoutingKey + uri.AbsolutePath
                }.Uri;
                rewrittenUrls.Add(uriRewrite);
            }
        }
        return Ok(rewrittenUrls);
    }
    
}