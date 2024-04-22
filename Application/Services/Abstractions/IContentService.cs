using Application.Dto;
using Domain.Entities;

namespace Application.Services.Abstractions;

public interface IContentService
{
    Task<ContentBase?> GetContentByIdAsync(long id);
    Task<MovieContent?> GetMovieContentByIdAsync(long id);
    Task<SerialContent?> GetSerialContentByIdAsync(long id);
    Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter);
    Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, int subscriptionId);
    Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution, int subscriptionId);
    Task DeleteContent(long contentId);
    Task UpdateMovieContent(MovieContentAdminPageDto movieContentAdminPageDto);
    Task UpdateSerialContent(SerialContentAdminPageDto serialContentDto);
    Task AddMovieContent(MovieContentAdminPageDto movieContentAdminPageDto);
    Task AddSerialContent(SerialContentAdminPageDto serialContentDto);
    Task<List<SectionDto>> GetSectionsAsync();
}