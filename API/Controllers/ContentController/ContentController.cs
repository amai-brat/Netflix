﻿using Domain.Abstractions;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.ContentController
{
    [Route("content")]
    [ApiController]
    public class ContentController(IContentService contentService) : ControllerBase
    {
        private readonly IContentService _contentService = contentService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentByIdAsync(long id)
        {
            var content = await _contentService.GetContentByIdAsync(id);

            if (content is null)
                return BadRequest(ErrorMessages.NotFoundContentError(id));

            content.PersonsInContent = content.PersonsInContent.GroupBy(p => p.ProfessionId)
                .SelectMany(p => p.Take(Consts.MaxReturnPersonPerRole))
                .ToList();

            return Ok(content);
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetContentsByFilterAsync([FromBody] Filter filter)
        {
            var contents = await _contentService.GetContentsByFilterAsync(filter);
            return Ok(contents);
        }

        [HttpPost("favourite/add")]
        [Authorize]
        public async Task<IActionResult> AddContentFavouriteAsync([FromBody] long contentId)
        {
            if (await _contentService.GetContentByIdAsync(contentId) is null)
                return BadRequest(ErrorMessages.NotFoundContentError(contentId));

            await _contentService.AddFavouriteAsync(contentId, long.Parse(User.FindFirst("Id")!.Value));
            return Ok();
        }

        [HttpPost("favourite/remove")]
        [Authorize]
        public async Task<IActionResult> RemoveContentFavouriteAsync([FromBody] long contentId)
        {
            if(await _contentService.GetContentByIdAsync(contentId) is null)
                return BadRequest(ErrorMessages.NotFoundContentError(contentId));

            await _contentService.RemoveFavouriteAsync(contentId, long.Parse(User.FindFirst("Id")!.Value));
            return Ok();
        }
    }
}