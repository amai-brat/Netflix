using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.Controllers;

[ApiController]
public class UploadController(
    IFileCopyService fileCopyService): ControllerBase
{
    [HttpGet("file/{guid}")]
    [Authorize]
    public async Task GetFile(Guid guid, CancellationToken cancellationToken)
    {
        if (!await fileCopyService.CanGetFileAsync(guid, 
                User.Claims.First(c => c.Type == ClaimTypes.Role).Value, 
                int.Parse(User.Claims.First(c => c.Type == "id").Value), 
                cancellationToken))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        Response.StatusCode = (int)HttpStatusCode.OK;
        Response.ContentType = "application/octet-stream";

        // https://stackoverflow.com/a/77105973/20734004
        // с amazon s3 клиентом можно было не писать так
        await fileCopyService.GetFileAsync(Response.Body ,guid, cancellationToken);
    }
    
    [HttpPost("copy-to-perm")]
    [Authorize]
    public async Task<IActionResult> Upload(
        List<Uri> guids,
        CancellationToken cancellationToken)
    {
        List<Uri> uriList = await fileCopyService.CopyFilesAsync(guids,cancellationToken);
        return Ok(uriList);
    }
}