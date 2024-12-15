using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SupportAPI.Filters;
using SupportAPI.Helpers;
using SupportAPI.Services;
using ContentDispositionHeaderValue = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace SupportAPI.Controller;

[ApiController]
public class FileUploadController(
    IFileUploadService fileUploadService,
    IHttpClientFactory clientFactory): ControllerBase
{
    [HttpPost("send-message-with-file"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
    [DisableFormValueModelBinding]
    [Authorize]
    public async Task<IActionResult> SendMessageWithFile(CancellationToken cancellationToken)
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
                    var result = await fileUploadService.UploadFileAndSaveMetadataAsync(currentStream);
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
            await fileUploadService.DeleteFileAndMetadataAsync(urls);
        }

        var rewrittenUrls = new List<Uri>();
        if (resp != null)
        {
            foreach (var uri in resp)
            {
                var uriRewrite = new UriBuilder(uri)
                {
                    Scheme = "https",
                    Host = "localhost",
                    Port = 443,
                    Path = uri.PathAndQuery
                }.Uri;
                
                rewrittenUrls.Add(uriRewrite);
            }
        }
        return Ok(rewrittenUrls);
    }
    
}