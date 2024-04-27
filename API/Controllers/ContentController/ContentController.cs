using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.Exceptions;
using Application.Services.Abstractions;
using AutoMapper;
using FluentValidation;

namespace API.Controllers.ContentController
{
    [Route("content")]
    [ApiController]
    public class ContentController(
        IContentService contentService,
        IFavouriteService favouriteService,
        IValidator<MovieContentAdminPageDto> movieContentAdminPageDtoValidator,
        IValidator<SerialContentAdminPageDto> serialContentAdminPageDtoValidator,
        IMapper mapper,
        IHttpClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentByIdAsync(long id)
        {
            var content = await contentService.GetContentByIdAsync(id);
            if (content is null)
                return BadRequest(ErrorMessages.NotFoundContent);

            if (content is SerialContent)
            {
                var serialContent = SetConstraintOnPersonCount((await contentService.GetSerialContentByIdAsync(id))!);
                var serealizedSerialContent = JsonSerializer.Serialize(serialContent,
                    new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                return Ok(serealizedSerialContent);
            }
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

        [HttpGet("movie/{id}/res/{resolution}/stream/chunk/output.ts")]
        [Authorize]
        public async Task<IActionResult> GetContentVideoStreamChunk(int id, int resolution)
        {
            // получаем id пользователя
            var userId = User.FindFirst("id")?.Value;
            if (userId is null)
                return StatusCode(StatusCodes.Status403Forbidden, "Нужен id пользователя");
            var userIdLong = long.Parse(userId);
            
            // получаем диапазон байтов
            var range = Request.Headers.Range.ToString();
            var start = 0L;
            var end = 0L;
            if (!string.IsNullOrEmpty(range))
            {
                var rangeParts = range.Replace("bytes=", "").Split('-');
                start = long.Parse(rangeParts[0]);
                end = rangeParts.Length > 1 && !string.IsNullOrEmpty(rangeParts[1]) ? long.Parse(rangeParts[1]) : 0;
            }
            // создаем http клиент
            var httpClient = clientFactory.CreateClient();
            var videoStreamUrl = await contentService.GetMovieContentStreamUrlAsync(userIdLong, id, resolution);
            var request = new HttpRequestMessage(HttpMethod.Get, videoStreamUrl);
            if (start != 0 || end != 0)
            {
                request.Headers.Range = new RangeHeaderValue(start, end);
            }
            
            // отправляем запрос
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Не удалось получить видео");
            }
            var contentLength = response.Content.Headers.ContentLength;
            if (!contentLength.HasValue)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось получить длину контента");
            }

            end = end == 0 ? contentLength.Value - 1 : end;

            var videoStream = await response.Content.ReadAsStreamAsync();
            Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{contentLength}");
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.Append("Content-Length", (end - start + 1).ToString());
            Response.StatusCode = (int)HttpStatusCode.PartialContent;
            // TODO: я не понимаю почему я не могу передать videoStream в File и сделать enableRangeProcessing:true
            // в таком случае он загружает файл полностью. приходится настраивать клиент чтобы он сам делал запросы на чанки
            // если кто-то поймет, скажите пж
            return new FileStreamResult(videoStream, "video/mp2t")
            {
                FileDownloadName = "output.ts"
            };
        }
        
        [HttpGet("movie/{id}/res/{resolution}/output.m3u8")]
        [Authorize]
        public async Task<IActionResult> GetContentVideoStream(int id, int resolution)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId is null)
                return StatusCode(StatusCodes.Status403Forbidden, "Нужен id пользователя");
            var userIdLong = long.Parse(userId);
            
            var videoStreamUrl = await contentService.GetMovieContentM3U8UrlAsync(userIdLong,id, resolution);
            
            var resp = await clientFactory.CreateClient().GetAsync(videoStreamUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!resp.IsSuccessStatusCode)
            {
                return StatusCode((int)resp.StatusCode, "Не удалось получить видео");
            }
            var videoStream = await resp.Content.ReadAsStreamAsync();
            
            
            return File(videoStream,"application/x-mpegURL",fileDownloadName:"output.m3u8");
        }

        [HttpGet("serial/{id}/season/{season}/episode/res/{resolution}/output.m3u8")]
        public async Task<IActionResult> GetContentSerialStream(int id, int resolution, int season, int episode)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId is null)
                return StatusCode(StatusCodes.Status403Forbidden, "Нужен id пользователя");

            var filePath = $"../MovieStorage/serial/{resolution}/{id}/{season}/{episode}/output.m3u8";
            var videoStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(videoStream,"application/x-mpegURL",fileDownloadName:"output.m3u8");
        }
        
        [HttpGet("serial/{id}/season/{season}/episode/{episode}/res/{resolution}/stream/chunk/output.ts")]  
        public async Task<IActionResult> GetContentSerialStreamChunk(int id, int resolution, int season, int episode)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId is null)
                return StatusCode(StatusCodes.Status403Forbidden, "Нужен id пользователя");

            var filePath = $"../MovieStorage/serial/{resolution}/{id}/{season}/{episode}/output.ts";
            var videoStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(videoStream,"video/mp2t",fileDownloadName:"output.ts", enableRangeProcessing:true );
        }
        
        [HttpPost("serial/add")]
        public async Task<IActionResult> AddSerialContent(SerialContentAdminPageDto serialContentAdminPageDto)
        {
            var validationResult = serialContentAdminPageDtoValidator.Validate(serialContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ToString());
            }
            await contentService.AddSerialContent(serialContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("serial/update/{id}")]
        public async Task<IActionResult> UpdateSerialContent(long id, SerialContentAdminPageDto serialContentAdminPageDto)
        {
            serialContentAdminPageDto.Id = id;
            var validationResult = serialContentAdminPageDtoValidator.Validate(serialContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ToString());
            }
            await contentService.UpdateSerialContent(serialContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("movie/add"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> AddMovieContent([FromForm] MovieContentAdminPageDto movieContentAdminPageDto)
        {
            var validationResult = movieContentAdminPageDtoValidator.Validate(movieContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ToString());
            }
            await contentService.AddMovieContent(movieContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("movie/update/{id}"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateMovieContent(long id,
            [FromForm] MovieContentAdminPageDto movieContentAdminPageDto)
        {
            movieContentAdminPageDto.Id = id;
            var validationResult = movieContentAdminPageDtoValidator.Validate(movieContentAdminPageDto);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ToString());
            }
            await contentService.UpdateMovieContent(movieContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            await contentService.DeleteContent(id);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet("admin/movie/{id}")]
        public async Task<MovieContentAdminPageDto> GetMovieContentAdminPageDto(long id)
        {
            var content = await contentService.GetContentByIdAsync(id);
            if (content is not MovieContent) throw new Exception("такого контента нет");
            
            var movieContent = await contentService.GetMovieContentByIdAsync(id);
            var movieContentDto = mapper.Map<MovieContentAdminPageDto>(movieContent);
            return movieContentDto;

        }
        [HttpGet("admin/serial/{id}")]
        public async Task<SerialContentAdminPageDto> GetSerialContentAdminPageDto(long id)
        {
            var content = await contentService.GetContentByIdAsync(id);
            if (content is not SerialContent) throw new Exception("такого контента нет");

            var serialContent = await contentService.GetSerialContentByIdAsync(id);
            var serialContentDto = mapper.Map<SerialContentAdminPageDto>(serialContent);
            return serialContentDto;
        }

        [HttpGet("sections")]
        public async Task<IActionResult> GetSections()
        {
            var result = await contentService.GetSectionsAsync();
            return Ok(result);
        }

        [HttpGet("promos")]
        public async Task<IActionResult> GetPromos()
        {
            var result = await contentService.GetPromosAsync();
            return Ok(result);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetContentTypes()
        {
            var result = await contentService.GetContentTypesAsync();
            return Ok(result);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetContentGenres()
        {
            var result = await contentService.GetGenresAsync();
            return Ok(result);
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