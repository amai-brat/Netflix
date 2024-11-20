using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Dto;
using Application.Exceptions.ErrorMessages;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentController(
        IContentService contentService,
        IFavouriteService favouriteService,
        IMapper mapper,
        IHttpClientFactory clientFactory) : ControllerBase
    {
        [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentByIdAsync(long id)
        {
            var content = await contentService.GetContentByIdAsync(id);
            if (content is null)
                return BadRequest(ErrorMessages.NotFoundContent);

            if (content is SerialContent)
            {
                var serialContent = SetConstraintOnPersonCount((await contentService.GetSerialContentByIdAsync(id))!);
                var serializedSerialContent = JsonSerializer.Serialize(serialContent,
                    new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                return Ok(serializedSerialContent);
            }
            else if(content is MovieContent)
                return Ok(SetConstraintOnPersonCount((await contentService.GetMovieContentByIdAsync(id))!));
            else 
                return Ok(SetConstraintOnPersonCount(content));
        }

        [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any)]
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
            
            // отправляем запрос на сервер с видео(minio)
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
            // получаем стрим и отдаем его клиенту
            var videoStream = await response.Content.ReadAsStreamAsync();
            Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{contentLength}");
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.Append("Content-Length", (end - start + 1).ToString());
            Response.StatusCode = (int)HttpStatusCode.PartialContent;
            return File(videoStream, "video/mp2t", fileDownloadName: "output.ts");
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

        [HttpGet("serial/{id}/season/{season}/episode/{episode}/res/{resolution}/output.m3u8")]
        public async Task<IActionResult> GetContentSerialStream(int id, int resolution, int season, int episode)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId is null)
                return StatusCode(StatusCodes.Status403Forbidden, "Нужен id пользователя");
            var userIdLong = long.Parse(userId);
            
            var videoStreamUrl = await contentService.GetSerialContentM3U8UrlAsync(userIdLong,id, season,episode,resolution);
            
            var resp = await clientFactory.CreateClient().GetAsync(videoStreamUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!resp.IsSuccessStatusCode)
            {
                return StatusCode((int)resp.StatusCode, "Не удалось получить видео");
            }
            var videoStream = await resp.Content.ReadAsStreamAsync();
            
            return File(videoStream,"application/x-mpegURL",fileDownloadName:"output.m3u8");
        }
        
        [HttpGet("serial/{id}/season/{season}/episode/{episode}/res/{resolution}/stream/chunk/output.ts")]  
        public async Task<IActionResult> GetContentSerialStreamChunk(int id, int resolution, int season, int episode)
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
            var videoStreamUrl = await contentService.GetSerialContentStreamUrlAsync(userIdLong,id, season,episode,resolution);
            var request = new HttpRequestMessage(HttpMethod.Get, videoStreamUrl);
            if (start != 0 || end != 0)
            {
                request.Headers.Range = new RangeHeaderValue(start, end);
            }
            
            // отправляем запрос на сервер с видео(minio)
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
            // получаем стрим и отдаем его клиенту
            var videoStream = await response.Content.ReadAsStreamAsync();
            Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{contentLength}");
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.Append("Content-Length", (end - start + 1).ToString());
            Response.StatusCode = (int)HttpStatusCode.PartialContent;
            return File(videoStream, "video/mp2t", fileDownloadName: "output.ts");
        }
        
        [HttpPost("serial/add"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> AddSerialContent([FromForm] SerialContentAdminPageDto serialContentAdminPageDto)
        {
            await contentService.AddSerialContent(serialContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("serial/update/{id}"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateSerialContent([FromRoute] long id, [FromForm] SerialContentAdminPageDto serialContentAdminPageDto)
        {
            await contentService.UpdateSerialContent(serialContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("movie/add"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> AddMovieContent([FromForm] MovieContentAdminPageDto movieContentAdminPageDto)
        {
            await contentService.AddMovieContent(movieContentAdminPageDto);
            return Ok();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("movie/update/{id}"), RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = Int32.MaxValue),DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateMovieContent([FromRoute] long id,
            [FromForm] MovieContentAdminPageDto movieContentAdminPageDto)
        {
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

        [Authorize(Roles = "admin")]
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
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetSections()
        {
            var result = await contentService.GetSectionsAsync();
            return Ok(result);
        }

        [HttpGet("promos")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetPromos()
        {
            var result = await contentService.GetPromosAsync();
            return Ok(result);
        }

        [HttpGet("types")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetContentTypes()
        {
            var result = await contentService.GetContentTypesAsync();
            return Ok(result);
        }

        [HttpGet("genres")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
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
            content.Genres.ForEach(g => g.Contents = null!);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if(content.ContentType != null)
                content.ContentType.ContentsWithType = null;
            content.PersonsInContent.ForEach(p => p.Content = null!);
            content.AllowedSubscriptions.ForEach(a => a.AccessibleContent = null!);
        }
    }
}