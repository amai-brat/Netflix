using Microsoft.AspNetCore.Http;

namespace Application.Services.Abstractions;

public interface IContentVideoManager
{
    Task<string> GetMovieContentM3U8UrlAsync(long movieId, int resolution);
    Task<string> GetSerialContentM3U8UrlAsync(long serialId, int seasonNumber, int episodeNumber, int resolution);
    Task<string> GetMovieContentStreamUrlAsync(long movieId, int resolution);
    Task<string> GetSerialContentStreamUrlAsync(long serialId, int seasonNumber, int episodeNumber, int resolution);
    Task PutMovieContentVideoAsync(long movieId, int resolution, IFormFile videoStream);
    Task PutSerialContentVideoAsync(long serialId, int resolution, int seasonNumber, int episodeNumber, IFormFile videoStream);

}