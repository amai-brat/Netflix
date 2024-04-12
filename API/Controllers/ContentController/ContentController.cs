using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Dto;
using Application.Exceptions;
using Application.Services.Abstractions;
using AutoMapper;
using FluentValidation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.ContentController
{
    [Route("content")]
    [ApiController]
    public class ContentController(
        IContentService contentService,
        IFavouriteService favouriteService,
        IValidator<MovieContentAdminPageDto> movieContentAdminPageDtoValidator,
        IValidator<SerialContentAdminPageDto> serialContentAdminPageDtoValidator) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentByIdAsync(long id)
        {
            var content = await contentService.GetContentByIdAsync(id);
            if (content is null)
                return BadRequest(ErrorMessages.NotFoundContent);

            if (content is SerialContent)
                return Ok(SetConstraintOnPersonCount((await contentService.GetSerialContentByIdAsync(id))!));
            else if(content is MovieContent)
                return Ok(SetConstraintOnPersonCount((await contentService.GetMovieContentByIdAsync(id))!));
            else 
                return Ok(SetConstraintOnPersonCount(content));
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetContentsByFilterAsync([FromQuery] Filter filter)
        {
            var contents = await contentService.GetContentsByFilterAsync(filter);
            return Ok(contents);
        }

        [HttpPost("favourite/add")]
        [Authorize]
        public async Task<IActionResult> AddContentFavouriteAsync([FromBody] long contentId)
        {
            await favouriteService.AddFavouriteAsync(contentId, long.Parse(User.FindFirst("Id")!.Value));
            return Ok();
        }

        [HttpPost("favourite/remove")]
        [Authorize]
        public async Task<IActionResult> RemoveContentFavouriteAsync([FromBody] long contentId)
        {
            await favouriteService.RemoveFavouriteAsync(contentId, long.Parse(User.FindFirst("Id")!.Value));
            return Ok();
        }

        [HttpGet("movie/video/{id}")]
        [Authorize]
        public async Task<IActionResult> GetContentVideo(long id, [FromQuery] int resolution)
        {
            var subscribeId = User.FindFirst("SubscribeId")?.Value;
            if (subscribeId is null)
                return Forbid(ErrorMessages.UserDoesNotHaveSubscription);

            var url = await contentService.GetMovieContentVideoUrlAsync(id, resolution, int.Parse(subscribeId));
            await HttpContext.Response.SendFileAsync(url);
            return Ok();
        }

        [HttpGet("serial/{season}/{episode}/video/{id}")]
        [Authorize]
        public async Task<IActionResult> GetContentVideo(int season, int episode, long id, [FromQuery] int resolution)
        {
            var subscribeId = User.FindFirst("SubscribeId")?.Value;
            if (subscribeId is null)
                return Forbid(ErrorMessages.UserDoesNotHaveSubscription);

            var url = await contentService.GetSerialContentVideoUrlAsync(id, season, episode, resolution, int.Parse(subscribeId));
            await HttpContext.Response.SendFileAsync(url);
            return Ok();
        }
        // TODO: авторизация
        [HttpPost("serial/add")]
        public async Task<IActionResult> AddSerialContent(SerialContentAdminPageDto serialContentAdminPageDto)
        {
            var validationResult = serialContentAdminPageDtoValidator.Validate(serialContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await contentService.AddSerialContent(serialContentAdminPageDto);
            return Ok();
        }
        // TODO: авторизация
        [HttpPost("serial/update/{id}")]
        public async Task<IActionResult> UpdateSerialContent(long id, SerialContentAdminPageDto serialContentAdminPageDto)
        {
            serialContentAdminPageDto.Id = id;
            var validationResult = serialContentAdminPageDtoValidator.Validate(serialContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await contentService.UpdateSerialContent(serialContentAdminPageDto);
            return Ok();
        }
        // TODO: авторизация
        [HttpPost("movie/add")]
        public async Task<IActionResult> AddMovieContent(MovieContentAdminPageDto movieContentAdminPageDto)
        {
            var validationResult = movieContentAdminPageDtoValidator.Validate(movieContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await contentService.AddMovieContent(movieContentAdminPageDto);
            return Ok();
        }
        // TODO: авторизация
        [HttpPost("movie/update/{id}")]
        public async Task<IActionResult> UpdateMovieContent(long id, MovieContentAdminPageDto movieContentAdminPageDto)
        {
            movieContentAdminPageDto.Id = id;
            var validationResult = movieContentAdminPageDtoValidator.Validate(movieContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await contentService.UpdateMovieContent(movieContentAdminPageDto);
            return Ok();
        }
        // TODO: авторизация
        [HttpPost("content/delete/{id}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            await contentService.DeleteContent(id);
            return Ok();
        }
        // [HttpPost("/test")]
        // public async Task<IActionResult> TestMethod()
        // {
        //     return Ok();
        // }
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