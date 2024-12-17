using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.Controllers;

[ApiController]
public class UploadController(
    IFileCopyService fileCopyService): ControllerBase
{
    
    [HttpPost("copy-to-perm")]
    [Authorize]
    public async Task<IActionResult> Upload(
        List<Uri> guids,
        CancellationToken cancellationToken)
    {
        List<Uri> uriList = await fileCopyService.CopyFilesAsync(guids,cancellationToken);
        return Ok(uriList);
    }

    [HttpGet("/get-file-uris")]
    [Authorize]
    public async Task<IActionResult> GetUris(Guid guid, CancellationToken cancellationToken)
    {
        if (!await fileCopyService.CanGetFileAsync(guid, 
                User.Claims.First(c => c.Type == ClaimTypes.Role).Value, 
                int.Parse(User.Claims.First(c => c.Type == "id").Value), 
                cancellationToken))
        {
            return Unauthorized();
        }
        var uri = await fileCopyService.GetPresignedUriAsync(guid, cancellationToken);
        var uriRewrite = new UriBuilder(uri)
        {
            Scheme = "http",
            Host = "localhost",
            Port = 9000,
            Path = "/perm-s3" + uri.AbsolutePath
        }.Uri;
        return Ok(uriRewrite);
    }
}