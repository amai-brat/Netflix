using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Shared.MessageContracts;
using SupportAPI.Models.Dto;
using SupportAPI.Services;

namespace SupportAPI.Controller;

[ApiController]
public class FileUploadController(
    IFileUploadService fileUploadService,
    IPublishEndpoint publishEndpoint): ControllerBase
{
    [HttpPost("send-message-with-file")]
    [Authorize]
    public async Task<IActionResult> SendMessageWithFile(
        long sessionId,
        [FromForm] UploadMessageWithFIleDto uploadMessageWithFIleDto)
    {
        var fileGuids = await fileUploadService.UploadFileAsync(uploadMessageWithFIleDto);

        await publishEndpoint.Publish(new FilesAndMetadatasUploaded(sessionId ,fileGuids));
        return Ok();
    }
    
}