using Application.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Abstractions;

public interface IContentService
{
    Task<ContentBase?> GetContentByIdAsync(long id);
    Task<MovieContent?> GetMovieContentByIdAsync(long id);
    Task<SerialContent?> GetSerialContentByIdAsync(long id);
    Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter);
    Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, int subscriptionId);
    Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, List<int> subscriptionIds);
    Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution, int subscriptionId);
    Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution, List<int> subscriptionIds);
    Task<bool> CheckIfContentAllowedWithSubscriptionIdAsync(long contentId, List<int> subscriptionIds);
    Task<string> GetMovieContentM3U8UrlAsync(long userId,long movieId, int resolution);
    Task<string> GetSerialContentM3U8UrlAsync(long userId,long serialId, int seasonNumber, int episodeNumber ,int resolution);
    Task<string> GetMovieContentStreamUrlAsync(long userId,long movieId, int resolution);
    Task<string> GetSerialContentStreamUrlAsync(long userId,long serialId, int seasonNumber, int episodeNumber, int resolution);


    Task DeleteContent(long contentId);
    Task UpdateMovieContent(MovieContentAdminPageDto movieContentAdminPageDto);
    Task UpdateSerialContent(SerialContentAdminPageDto serialContentDto);
    Task AddMovieContent(MovieContentAdminPageDto movieContentAdminPageDto);
    Task AddSerialContent(SerialContentAdminPageDto serialContentDto);
    Task<List<SectionDto>> GetSectionsAsync();
    Task<List<PromoDto>> GetPromosAsync();
    Task<List<ContentTypeDto>> GetContentTypesAsync();
    Task<List<GenreDto>> GetGenresAsync();
}