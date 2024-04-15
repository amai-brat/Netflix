﻿using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.ContentController
{
    [Route("content")]
    [ApiController]
    public class ContentController(
        IContentService contentService,
        IFavouriteService favouriteService
        ) : ControllerBase
    {
        private readonly IContentService _contentService = contentService;
        private readonly IFavouriteService _favouriteService = favouriteService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentByIdAsync(long id)
        {
            var content = await _contentService.GetContentByIdAsync(id);
            if (content is null)
                return BadRequest(ErrorMessages.NotFoundContent);

            if (content is SerialContent)
                return Ok(SetConstraintOnPersonCount((await _contentService.GetSerialContentByIdAsync(id))!));
            else if(content is MovieContent)
                return Ok(SetConstraintOnPersonCount((await _contentService.GetMovieContentByIdAsync(id))!));
            else 
                return Ok(SetConstraintOnPersonCount(content));
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetContentsByFilterAsync([FromQuery] Filter filter)
        {
            var contents = await _contentService.GetContentsByFilterAsync(filter);
            return Ok(contents);
        }

        [HttpPost("favourite/add")]
        [Authorize]
        public async Task<IActionResult> AddContentFavouriteAsync([FromBody] long contentId)
        {
            await _favouriteService.AddFavouriteAsync(contentId, long.Parse(User.FindFirst("Id")!.Value));
            return Ok();
        }

        [HttpPost("favourite/remove")]
        [Authorize]
        public async Task<IActionResult> RemoveContentFavouriteAsync([FromBody] long contentId)
        {
            await _favouriteService.RemoveFavouriteAsync(contentId, long.Parse(User.FindFirst("Id")!.Value));
            return Ok();
        }

        [HttpGet("movie/video/{id}")]
        [Authorize]
        public async Task<IActionResult> GetContentVideo(long id, [FromQuery] int resolution)
        {
            var subscribeId = User.FindFirst("SubscribeId")?.Value;
            if (subscribeId is null)
                return Forbid(ErrorMessages.UserDoesNotHaveSubscription);

            var range = HttpContext.Request.Headers.Range;
            if (string.IsNullOrEmpty(range))
                range = "bytes=0-";

            var videoPath = await _contentService.GetMovieContentVideoUrlAsync(id, resolution, int.Parse(subscribeId));

            var videoSize = new FileInfo(videoPath).Length;
            var start = long.Parse(range.First()!.Where(char.IsDigit).ToArray());
            var end = Math.Min(start + Consts.ChunkSize, videoSize - 1);
            var contentLength = end - start + 1;

            Response.Headers.ContentRange = $"bytes {start}-{end}/{videoSize}";
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.ContentLength = contentLength;
            Response.Headers.ContentType = "video/mp4";

            var videoStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(videoStream,"video/mp4", enableRangeProcessing:true );
        }

        [HttpGet("serial/{season}/{episode}/video/{id}")]
        [Authorize]
        public async Task<IActionResult> GetContentVideo(int season, int episode, long id, [FromQuery] int resolution)
        {
            var subscribeId = User.FindFirst("SubscribeId")?.Value;
            if (subscribeId is null)
                return Forbid(ErrorMessages.UserDoesNotHaveSubscription);

            var range = HttpContext.Request.Headers.Range;
            if (string.IsNullOrEmpty(range))
                range = "bytes=0-";
            
            var videoPath = await _contentService.GetSerialContentVideoUrlAsync(id, season, episode, resolution, int.Parse(subscribeId));
            
            var videoSize = new FileInfo(videoPath).Length;
            var start = long.Parse(range.First()!.Where(char.IsDigit).ToArray());
            var end = Math.Min(start + Consts.ChunkSize, videoSize - 1);
            var contentLength = end - start + 1;

            Response.Headers.ContentRange = $"bytes {start}-{end}/{videoSize}";
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.ContentLength = contentLength;
            Response.Headers.ContentType = "video/mp4";

            var videoStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(videoStream,"video/mp4", enableRangeProcessing:true );
        }

        private T SetConstraintOnPersonCount<T>(T content) where T : ContentBase
        {
            content.PersonsInContent = content.PersonsInContent.GroupBy(p => p.ProfessionId)
                .SelectMany(p => p.Take(Consts.MaxReturnPersonPerRole))
                .ToList();
            SetUpContent(content);
            return content;
        }

        private void SetUpContent(ContentBase content)
        {
            content.Genres?.ForEach(g => g.Contents = null!);
            if(content.ContentType != null)
                content.ContentType.ContentsWithType = null;
            content.PersonsInContent?.ForEach(p => p.Content = null!);
            content.AllowedSubscriptions?.ForEach(a => a.AccessibleContent = null!);
        }
    }
}