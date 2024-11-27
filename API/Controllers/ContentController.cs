using API.Helpers;
using Application.Features.Contents.Commands.AddMovieContent;
using Application.Features.Contents.Commands.AddSerialContent;
using Application.Features.Contents.Commands.DeleteContent;
using Application.Features.Contents.Commands.UpdateMovieContent;
using Application.Features.Contents.Commands.UpdateSerialContent;
using Application.Features.Contents.Dtos;
using Application.Features.Contents.Queries.Admin.GetMovieContent;
using Application.Features.Contents.Queries.Admin.GetSerialContent;
using Application.Features.Contents.Queries.GetContent;
using Application.Features.Contents.Queries.GetContentsByFilter;
using Application.Features.Contents.Queries.GetContentTypes;
using Application.Features.Contents.Queries.GetGenres;
using Application.Features.Contents.Queries.GetPromos;
using Application.Features.Contents.Queries.GetSections;
using Application.Features.Contents.Queries.Streaming.GetMovieContentSteam;
using Application.Features.Contents.Queries.Streaming.GetMovieContentStreamChunk;
using Application.Features.Contents.Queries.Streaming.GetSerialContentStream;
using Application.Features.Contents.Queries.Streaming.GetSerialContentStreamChunk;
using Application.Features.Favourites.Commands.AddFavourite;
using Application.Features.Favourites.Commands.RemoveFavourite;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentController(
        IMediator mediator) : ControllerBase
    {
        [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentByIdAsync(long id)
        {
            var result = await mediator.Send(new GetContentQuery(id));
            return Ok(result);
        }
        [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any)]
        [HttpGet("filter")]
        public async Task<IActionResult> GetContentsByFilterAsync([FromQuery] Filter filter)
        {
            var result = await mediator.Send(new GetContentsByFilterQuery(filter));
            return Ok(result.Contents);
        }

        [HttpPost("favourite/add")]
        [Authorize]
        public async Task<IActionResult> AddContentFavouriteAsync([FromBody] long contentId)
        {
            var userId = this.GetUserId();
            await mediator.Send(new AddFavouriteCommand(contentId, userId));
            return Ok();
        }

        [HttpPost("favourite/remove")]
        [Authorize]
        public async Task<IActionResult> RemoveContentFavouriteAsync([FromBody] long contentId)
        {
            var userId = this.GetUserId();
            await mediator.Send(new RemoveFavouriteCommand(contentId, userId));
            return Ok();
        }

        [HttpGet("movie/{id}/res/{resolution}/stream/chunk/output.ts")]
        [Authorize]
        public async Task<IActionResult> GetContentVideoStreamChunk(int id, int resolution)
        {
            var userId = this.GetUserId();
            var result = await mediator.Send(new GetMovieContentStreamChunkQuery(userId, id, resolution));
            
            if (result.VideoStream is not null)
            {
                return File(result.VideoStream, "video/mp2t", fileDownloadName: "output.ts");
            }

            return StatusCode(result.ErrorCode, result.Error);
        }
        
        [HttpGet("movie/{id}/res/{resolution}/output.m3u8")]
        [Authorize]
        public async Task<IActionResult> GetContentVideoStream(int id, int resolution)
        {
            var userId = this.GetUserId();
            var result = await mediator.Send(new GetMovieContentStreamQuery(userId, id, resolution));

            if (result.VideoStream is not null)
            {
                return File(result.VideoStream,"application/x-mpegURL", fileDownloadName:"output.m3u8");
            }

            return StatusCode(result.ErrorCode, result.Error);
        }

        [HttpGet("serial/{id}/season/{season}/episode/{episode}/res/{resolution}/output.m3u8")]
        public async Task<IActionResult> GetContentSerialStream(int id, int resolution, int season, int episode)
        {
            var userId = this.GetUserId();
            var result = await mediator.Send(
                new GetSerialContentStreamQuery(userId, id, season, episode, resolution));

            if (result.VideoStream is not null)
            {
                return File(result.VideoStream,"application/x-mpegURL",fileDownloadName:"output.m3u8");
            }
            
            return StatusCode(result.ErrorCode, result.Error);
        }
        
        [HttpGet("serial/{id}/season/{season}/episode/{episode}/res/{resolution}/stream/chunk/output.ts")]  
        public async Task<IActionResult> GetContentSerialStreamChunk(int id, int resolution, int season, int episode)
        {
            var userId = this.GetUserId();
            var result = await mediator.Send(
                new GetSerialContentStreamChunkQuery(userId, id, season, episode, resolution));
            
            if (result.VideoStream is not null)
            {
                return File(result.VideoStream, "video/mp2t", fileDownloadName: "output.ts");
            }

            return StatusCode(result.ErrorCode, result.Error);
        }
        
        [HttpPost("serial/add"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> AddSerialContent([FromForm] SerialContentDto serialContentDto)
        {
            await mediator.Send(new AddSerialContentCommand(serialContentDto));
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("serial/update/{id}"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateSerialContent([FromRoute] long id, [FromForm] SerialContentDto serialContentDto)
        {
            await mediator.Send(new UpdateSerialContentCommand(serialContentDto));
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("movie/add"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> AddMovieContent([FromForm] MovieContentDto movieContentDto)
        {
            await mediator.Send(new AddMovieContentCommand(movieContentDto));
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("movie/update/{id}"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateMovieContent([FromRoute] long id,
            [FromForm] MovieContentDto movieContentDto)
        {
            await mediator.Send(new UpdateMovieContentCommand(movieContentDto));
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            await mediator.Send(new DeleteContentCommand(id));
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet("admin/movie/{id}")]
        public async Task<IActionResult> GetMovieContentAdminPageDto(long id)
        {
            var result = await mediator.Send(new GetMovieContentQuery(id));
            return Ok(result);
        }
        
        [HttpGet("admin/serial/{id}")]
        public async Task<IActionResult> GetSerialContentAdminPageDto(long id)
        {
            var result = await mediator.Send(new GetSerialContentQuery(id));
            return Ok(result);
        }

        [HttpGet("sections")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetSections()
        {
            var result = await mediator.Send(new GetSectionsQuery());
            return Ok(result.SectionDtos);
        }

        [HttpGet("promos")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetPromos()
        {
            var result = await mediator.Send(new GetPromosQuery());
            return Ok(result.PromoDtos);
        }

        [HttpGet("types")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetContentTypes()
        {
            var result = await mediator.Send(new GetContentTypesQuery());
            return Ok(result.ContentTypeDtos);
        }

        [HttpGet("genres")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetContentGenres()
        {
            var result = await mediator.Send(new GetGenresQuery());
            return Ok(result.GenreDtos);
        }
    }
}