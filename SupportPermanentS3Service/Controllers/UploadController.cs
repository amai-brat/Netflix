﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SupportPermanentS3Service.Options;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.Controllers;

[ApiController]
public class UploadController(
    IFileCopyService fileCopyService,
    IOptions<MinioProxyOptions> proxyOptions): ControllerBase
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
        return uri is not null
            ? Ok(GetProxiedUri(uri))
            : Ok(GetProcessingImageUri());
    }

    private Uri GetProxiedUri(Uri uri)
    {
        var options = proxyOptions.Value;
        return new UriBuilder(uri)
        {
            Scheme = options.Scheme,
            Host = options.Host,
            Port = options.Port,
            Path = options.RoutingKey + uri.AbsolutePath
        }.Uri;
    }

    private Uri GetProcessingImageUri()
    {
        // var builder =  new UriBuilder
        // {
        //     Scheme = Request.Scheme,
        //     Host = Request.Host.Host,
        //     Path = "/imgs/processing.png",
        // };
        //
        // if (Request.Host.Port.HasValue)
        // {
        //     builder.Port = Request.Host.Port.Value;
        // }
        //
        // return builder.Uri;

        return new Uri("https://i.imgur.com/QtmQQrc.png");
    }
}