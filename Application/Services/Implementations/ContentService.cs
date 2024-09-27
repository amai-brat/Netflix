using System.Diagnostics;
using System.Linq.Expressions;
using Application.Dto;
using Application.Exceptions.ErrorMessages;
using Application.Exceptions.Particular;
using Application.Repositories;
using Application.Services.Abstractions;
using Application.Services.Extensions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Implementations;

public class ContentService(
    IContentRepository contentRepository,
    ISubscriptionRepository subscriptionRepository,
    IContentTypeRepository contentTypeRepository,
    IGenreRepository genreRepository,
    IContentVideoProvider contentVideoProvider,
    IUserRepository userRepository,
    IValidator<MovieContentAdminPageDto> movieContentAdminPageDtoValidator,
    IValidator<SerialContentAdminPageDto> serialContentAdminPageDtoValidator,
    IMapper mapper) : IContentService
{
    private readonly HashSet<int> _resolutions = [360, 480, 720, 1080, 1440, 2160];
    public async Task<string> GetMovieContentM3U8UrlAsync(long userId, long movieId, int resolution)
    {
        var userSubscriptions = (await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == userId))
            ?.UserSubscriptions?.Select(s => s.SubscriptionId).ToList();
        var userCanViewContent = await CheckIfContentAllowedWithSubscriptionIdAsync(movieId,userSubscriptions);
        if (!userCanViewContent)
        {
            throw new ContentServiceNotPermittedException("Вам нужна подписка чтобы смотреть этот контент");
        }
        var resource = await GetMovieVideoUrlAsync(movieId, resolution)  + ".m3u8";
        return await contentVideoProvider.GetUrlAsync(resource);
    }

    public async Task<string> GetSerialContentM3U8UrlAsync(long userId, long serialId, int seasonNumber, int episodeNumber, int resolution)
    {
        var userSubscriptions = (await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == userId))?
            .UserSubscriptions?.Select(s => s.SubscriptionId).ToList();
        var userCanViewContent = await CheckIfContentAllowedWithSubscriptionIdAsync(serialId,userSubscriptions);
        if (!userCanViewContent)
        {
            throw new ContentServiceNotPermittedException("Вам нужна подписка чтобы смотреть этот контент");
        }

        var serialUrl = await GetSerialEpisodeVideoUrlAsync(serialId, resolution, seasonNumber, episodeNumber) + ".m3u8";
        return await contentVideoProvider.GetUrlAsync(serialUrl);
    }

    public async Task<string> GetMovieContentStreamUrlAsync(long userId, long movieId, int resolution)
    {
        var userSubscriptions = (await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == userId))?
            .UserSubscriptions?.Select(s => s.SubscriptionId).ToList();
        var userCanViewContent = await CheckIfContentAllowedWithSubscriptionIdAsync(movieId,userSubscriptions);
        if (!userCanViewContent)
        {
            throw new ContentServiceNotPermittedException("Вам нужна подписка чтобы смотреть этот контент");
        }
        var resource = await GetMovieVideoUrlAsync(movieId, resolution)  + ".ts";

        return await contentVideoProvider.GetUrlAsync(resource);
    }

    public async Task<string> GetSerialContentStreamUrlAsync(long userId, long serialId, int seasonNumber, int episodeNumber, int resolution)
    {
        var userSubscriptions = (await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == userId))?
            .UserSubscriptions?.Select(s => s.SubscriptionId).ToList();
        var userCanViewContent = await CheckIfContentAllowedWithSubscriptionIdAsync(serialId,userSubscriptions);
        if (!userCanViewContent)
        {
            throw new ContentServiceNotPermittedException("Вам нужна подписка чтобы смотреть этот контент");
        }

        var serialUrl = await GetSerialEpisodeVideoUrlAsync(serialId, resolution, seasonNumber, episodeNumber) + ".ts";
        return await contentVideoProvider.GetUrlAsync(serialUrl);
    }

    private async Task PutMovieContentVideoAsync(long movieId, int resolution, IFormFile videoStream)
    {
        // сохраняет видео из формы временно на сервере
        var tempFilePath = await SaveVideoToTempFile(videoStream);
        
        // берем сохраненное видео и генерируем из него .m3u8 и .ts файлы, возвращает путь к этим файлам
        var pathToGeneratedHlsFiles = await ConvertMovieFileToHls(tempFilePath, movieId, resolution);
        
        // эти файлы сохраняем в minio
        await SaveMovieHlsFilesToMinio(pathToGeneratedHlsFiles, movieId, resolution);
    }
    private async Task PutSerialContentVideoAsync(long serialId, int resolution, int seasonNumber,int episodeNumber,IFormFile videoStream)
    {
        // сохраняет видео из формы временно на сервере
        var tempFilePath = await SaveVideoToTempFile(videoStream);
        
        // берем сохраненное видео и генерируем из него .m3u8 и .ts файлы, возвращает путь к этим файлам
        var pathToGeneratedHlsFiles = await ConvertSerialFileToHls(tempFilePath,serialId,resolution,seasonNumber,episodeNumber);
        
        // эти файлы сохраняем в minio
        await SaveSerialHlsFilesToMinie(pathToGeneratedHlsFiles,serialId,resolution,seasonNumber,episodeNumber);
    }
    private async Task<string> SaveVideoToTempFile(IFormFile file)
    {
        var tempFilePath = Path.GetTempFileName();

        await using var stream = new FileStream(tempFilePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return tempFilePath;
    }
    // берет сохраненный сериал из него .m3u8 и .ts файлы, возвращает путь к этим файлам
    private async Task<string> ConvertSerialFileToHls(string inputPath, long id, int resolution, int seasonNumber,
        int episodeNumber)
    {
        string outputPath = Path.Combine(Path.GetTempPath(), $"content/{id}/res/{resolution}/season/{seasonNumber}/episode/{episodeNumber}");
        Directory.CreateDirectory(outputPath);

        var hlsBaseUrl = Environment.GetEnvironmentVariable("HLS_BASE_URL") ?? "http://localhost:8080";
        if (hlsBaseUrl is null)
        {
            throw new ContentServiceArgumentException("HLS_BASE_URL env variable not set", "");
        }
        
        var resolutionString = GetResolutionByIntValue(resolution);
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $"-i {inputPath} -c:v libx264 -profile:v high -level 4.0 -s {resolutionString} -start_number 0" +
                        $" -hls_time 5 -hls_list_size 0 -hls_base_url \"{hlsBaseUrl}/content/serial/{id}/season/{seasonNumber}" +
                        $"/episode/{episodeNumber}/res/{resolution}/stream/chunk/\"" +
                        $" -hls_segment_filename {outputPath + "/output.ts"} -hls_flags single_file -f hls {outputPath + "/output.m3u8"}",
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = new Process();
        process.StartInfo = startInfo;
        try
        {
            process.Start();
        }
        catch(Exception)
        {
            Directory.Delete(outputPath,true);
        }
        
        await process.WaitForExitAsync();
        return outputPath;
    }
        
    // берет сохраненный фильм и генерирует из него .m3u8 и .ts файлы, возвращает путь к этим файлам
    private async Task<string> ConvertMovieFileToHls(string inputPath, long id, int resolution)
    {
        string outputPath = Path.Combine(Path.GetTempPath(), $"content/{id}/res/{resolution}");
        Directory.CreateDirectory(outputPath);

        var hlsBaseUrl = Environment.GetEnvironmentVariable("HLS_BASE_URL");
        if (hlsBaseUrl is null)
        {
            throw new ContentServiceArgumentException("HLS_BASE_URL env variable not set", "");
        }
        
        var resolutionString = GetResolutionByIntValue(resolution);
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $"-i {inputPath} -c:v libx264 -profile:v high -level 4.0 -s {resolutionString} -start_number 0" +
                        $" -hls_time 5 -hls_list_size 0 -hls_base_url \"{hlsBaseUrl}/content/movie/{id}/res/{resolution}/stream/chunk/\"" +
                        $" -hls_segment_filename {outputPath + "/output.ts"} -hls_flags single_file -f hls {outputPath + "/output.m3u8"}",
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = new Process();
        process.StartInfo = startInfo;
        try
        {
            process.Start();
        }
        catch(Exception)
        {
            Directory.Delete(outputPath,true);
        }
        
        
        await process.WaitForExitAsync();
        return outputPath;
    }

    private async Task SaveMovieHlsFilesToMinio(string pathToFiles, long id, int resolution)
    {
        try
        {
            await using var m3U8Stream = new FileStream(Path.Combine(pathToFiles,"output.m3u8"), FileMode.Open);
            await using var tsStream = new FileStream(Path.Combine(pathToFiles,"output.ts"), FileMode.Open);
            await contentVideoProvider.PutAsync($"/movie/{id}/res/{resolution}/output.m3u8", m3U8Stream,
                "application/x-mpegURL");
            await contentVideoProvider.PutAsync($"/movie/{id}/res/{resolution}/output.ts", tsStream, "video/mp2t");
        }
        finally
        {
            // удаляем временные файлы
            Directory.Delete(pathToFiles,true); 
        }
    }

    private async Task SaveSerialHlsFilesToMinie(string pathToFiles, long id, int resolution, int seasonNumber,
        int episodeNumber)
    {
        try
        {

            await using var m3U8Stream = new FileStream(Path.Combine(pathToFiles,"output.m3u8"), FileMode.Open);
            await using var tsStream = new FileStream(Path.Combine(pathToFiles,"output.ts"), FileMode.Open);
            await contentVideoProvider.PutAsync($"/serial/{id}/season/{seasonNumber}/episode/{episodeNumber}/res/{resolution}/output.m3u8", m3U8Stream,
                "application/x-mpegURL");
            await contentVideoProvider.PutAsync($"/serial/{id}/season/{seasonNumber}/episode/{episodeNumber}/res/{resolution}/output.ts", tsStream, "video/mp2t");
        }
        finally
        {
            // удаляем временные файлы
            Directory.Delete(pathToFiles,true); 
        }
    }
    private string GetResolutionByIntValue(int resolution)
    {
        switch (resolution)
        {
            case 360: return "480x360";
            case 480: return "640x480";
            case 720: return "1280x720";
            case 1080: return "1920x1080";
            case 1440: return "2560x1440";
            case 2160: return "3840x2160";
            default: throw new Exception("Неподдерживаемое разрешение");
        }
    }
    public async Task<bool> CheckIfContentAllowedWithSubscriptionIdAsync(long contentId, List<int>? subscriptionIds)
    {
        var content = await contentRepository.GetContentWithAllowedSubscriptionsByIdAsync(contentId);
        if (content == null || subscriptionIds == null || subscriptionIds.Count == 0)
        {
            return false;
        }

        return content.AllowedSubscriptions.Any(s => subscriptionIds.Contains(s.Id));
    }
    public async Task<ContentBase?> GetContentByIdAsync(long id) =>
        await contentRepository.GetContentByFilterAsync(c => c.Id == id);

    public async Task<MovieContent?> GetMovieContentByIdAsync(long id) =>
        await contentRepository.GetMovieContentByFilterAsync(c => c.Id == id);

    public async Task<SerialContent?> GetSerialContentByIdAsync(long id) =>
        await contentRepository.GetSerialContentByFilterAsync(c => c.Id == id);

    public async Task<List<ContentBase>> GetContentsByFilterAsync(Filter filter) =>
        await contentRepository.GetContentsByFilterAsync(
            IsContentNameContain(filter)
                .CombineExpressions(IsContentTypesContain(filter))
                .CombineExpressions(IsCountryContain(filter))
                .CombineExpressions(IsContentGenresContains(filter))
                .CombineExpressions(IsContentYearBetween(filter))
                .CombineExpressions(IsContentRatingBetween(filter))
        );

    private async Task<string> GetMovieVideoUrlAsync(long movieId, int resolution)
    {
        var movie = await contentRepository.GetMovieContentByFilterAsync(m => m.Id == movieId);
        if (movie is null)
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{movieId}");
        if (!_resolutions.Contains(resolution))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");

        string url = movie.VideoUrl;
        url = url.Replace("{res}", resolution.ToString());
        url = url.Replace("{id}", movieId.ToString());

        return url;
    }

    private async Task<string> GetSerialEpisodeVideoUrlAsync(long serialId, int resolution, int seasonNumber, int episodeNumber)
    {
        var serial = await contentRepository.GetSerialContentByFilterAsync(s => s.Id == serialId);
        if (serial is null)
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{serialId}");

        if (!_resolutions.Contains(resolution))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");

        if (!serial.SeasonInfos.Select(s => s.SeasonNumber)
                .Contains(seasonNumber))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundSeason, $"{seasonNumber}");

        if (!serial.SeasonInfos.Single(s => s.SeasonNumber == seasonNumber).Episodes
                .Select(e => e.EpisodeNumber)
                .Contains(episodeNumber))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundEpisode, $"{episodeNumber}");

        var url = serial
            .SeasonInfos
            .First(s => s.SeasonNumber == seasonNumber)
            .Episodes
            .First(e => e.EpisodeNumber == episodeNumber)
            .VideoUrl;
        url = url.Replace("{res}", resolution.ToString());
        url = url.Replace("{id}", serialId.ToString());
        url = url.Replace("{season}", seasonNumber.ToString());
        url = url.Replace("{episode}", episodeNumber.ToString());
        
        return url;
    }

    public async Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, int subscriptionId)
    {
        var movie = await contentRepository.GetMovieContentByFilterAsync(m => m.Id == movieId);
        if (movie is null)
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{movieId}");
        if (!_resolutions.Contains(resolution))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");
        if (!movie.AllowedSubscriptions.Select(s => s.Id)
                .Contains(subscriptionId) ||
            movie.AllowedSubscriptions.First(s => s.Id == subscriptionId).MaxResolution < resolution)
            throw new ContentServiceNotPermittedException(ErrorMessages.UserDoesNotHavePermissionBySubscription);

        return movie.VideoUrl.Replace("resolution", resolution.ToString());
    }
    
    public async Task<string> GetMovieContentVideoUrlAsync(long movieId, int resolution, List<int> subscriptionIds)
    {
        var movie = await contentRepository.GetMovieContentByFilterAsync(m => m.Id == movieId);
        if (movie is null)
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{movieId}");
        if (!_resolutions.Contains(resolution))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");

        var allowedSubscriptionIds = subscriptionIds
            .Where(subscriptionId =>
                !movie.AllowedSubscriptions
                    .Select(s => s.Id)
                    .Contains(subscriptionId))
            .ToList();
        
        if (allowedSubscriptionIds.Count > 0 ||
            allowedSubscriptionIds
                .Any(subscriptionId => movie.AllowedSubscriptions
                    .First(s => s.Id == subscriptionId).MaxResolution < resolution))
            throw new ContentServiceNotPermittedException(ErrorMessages.UserDoesNotHavePermissionBySubscription);

        return movie.VideoUrl.Replace("resolution", resolution.ToString());
    }

    public async Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution,
        int subscriptionId)
    {
        var serial = await contentRepository.GetSerialContentByFilterAsync(s => s.Id == serialId);
        if (serial is null)
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{serialId}");

        if (!_resolutions.Contains(resolution))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");

        if (!serial.SeasonInfos.Select(s => s.SeasonNumber)
                .Contains(season))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundSeason, $"{season}");

        if (!serial.SeasonInfos.Single(s => s.SeasonNumber == season).Episodes
                .Select(e => e.EpisodeNumber)
                .Contains(episode))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundEpisode, $"{episode}");

        if (!serial.AllowedSubscriptions.Select(s => s.Id)
                .Contains(subscriptionId) ||
            serial.AllowedSubscriptions.First(s => s.Id == subscriptionId).MaxResolution < resolution)
            throw new ContentServiceNotPermittedException(ErrorMessages.UserDoesNotHavePermissionBySubscription);

        return serial.SeasonInfos.Single(s => s.SeasonNumber == season).Episodes
            .Single(e => e.EpisodeNumber == episode).VideoUrl
            .Replace("resolution", resolution.ToString());
    }
    
    public async Task<string> GetSerialContentVideoUrlAsync(long serialId, int season, int episode, int resolution,
        List<int> subscriptionIds)
    {
        var serial = await contentRepository.GetSerialContentByFilterAsync(s => s.Id == serialId);
        if (serial is null)
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundContent, $"{serialId}");

        if (!_resolutions.Contains(resolution))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundResolution, $"{resolution}");

        if (!serial.SeasonInfos.Select(s => s.SeasonNumber)
                .Contains(season))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundSeason, $"{season}");

        if (!serial.SeasonInfos.Single(s => s.SeasonNumber == season).Episodes
                .Select(e => e.EpisodeNumber)
                .Contains(episode))
            throw new ContentServiceArgumentException(ErrorMessages.NotFoundEpisode, $"{episode}");

        
        var allowedSubscriptionIds = subscriptionIds
            .Where(subscriptionId =>
                !serial.AllowedSubscriptions
                    .Select(s => s.Id)
                    .Contains(subscriptionId))
            .ToList();
        
        if (allowedSubscriptionIds.Count > 0 ||
            allowedSubscriptionIds
                .Any(subscriptionId => serial.AllowedSubscriptions
                    .First(s => s.Id == subscriptionId).MaxResolution < resolution))
            throw new ContentServiceNotPermittedException(ErrorMessages.UserDoesNotHavePermissionBySubscription);

        return serial.SeasonInfos.Single(s => s.SeasonNumber == season).Episodes
            .Single(e => e.EpisodeNumber == episode).VideoUrl
            .Replace("resolution", resolution.ToString());
    }

    public async Task DeleteContent(long contentId)
    {
        contentRepository.DeleteContent(contentId);
        await contentRepository.SaveChangesAsync();
    }

    public async Task UpdateMovieContent(MovieContentAdminPageDto movieContentAdminPageDto)
    {
        var validationResult = movieContentAdminPageDtoValidator.Validate(movieContentAdminPageDto);
        if (!validationResult.IsValid)
        {
            throw new Exception(validationResult.ToString());
        }
        if (movieContentAdminPageDto.VideoFile != null)
        {
            await PutMovieContentVideoAsync(movieContentAdminPageDto.Id, movieContentAdminPageDto.Resolution, movieContentAdminPageDto.VideoFile);
        }
        var movieContent = mapper.Map<MovieContentAdminPageDto, MovieContent>(movieContentAdminPageDto);
        CheckIfSubscriptionsHaveNewOne(movieContent.AllowedSubscriptions, await subscriptionRepository.GetAllSubscriptionsAsync());
        await contentRepository.UpdateMovieContent(movieContent);
        await contentRepository.SaveChangesAsync();
    }

    public async Task UpdateSerialContent(SerialContentAdminPageDto serialContentDto)
    {
        var validationResult = serialContentAdminPageDtoValidator.Validate(serialContentDto);
        if (!validationResult.IsValid)
        {
            throw new Exception(validationResult.ToString());
        }
        var serialContent = mapper.Map<SerialContentAdminPageDto, SerialContent>(serialContentDto);
        CheckIfSubscriptionsHaveNewOne(serialContent.AllowedSubscriptions, await subscriptionRepository.GetAllSubscriptionsAsync());
        foreach (var season in serialContentDto.SeasonInfos)
        {
            foreach (var episode in season.Episodes)
            {
                if (episode.VideoFile != null)
                {
                    await PutSerialContentVideoAsync(serialContentDto.Id, episode.Resolution,
                        season.SeasonNumber, episode.EpisodeNumber, episode.VideoFile);
                }
            }
        }
        await contentRepository.UpdateSerialContent(serialContent);
        await contentRepository.SaveChangesAsync();
    }

    public async Task AddMovieContent(MovieContentAdminPageDto movieContentAdminPageDto)
    {
        var validationResult = movieContentAdminPageDtoValidator.Validate(movieContentAdminPageDto);
        if (!validationResult.IsValid)
        {
            throw new Exception(validationResult.ToString());
        }
        var movieContent = mapper.Map<MovieContentAdminPageDto, MovieContent>(movieContentAdminPageDto);
        CheckIfSubscriptionsHaveNewOne(movieContent.AllowedSubscriptions, await subscriptionRepository.GetAllSubscriptionsAsync());
        contentRepository.AddMovieContent(movieContent);
        await contentRepository.SaveChangesAsync();
        if (movieContentAdminPageDto.VideoFile != null)
        {
            await PutMovieContentVideoAsync(movieContent.Id, movieContentAdminPageDto.Resolution, movieContentAdminPageDto.VideoFile);
        }
    }

    public async Task AddSerialContent(SerialContentAdminPageDto serialContentDto)
    {
        var validationResult = serialContentAdminPageDtoValidator.Validate(serialContentDto);
        if (!validationResult.IsValid)
        {
            throw new Exception(validationResult.ToString());
        }
        var serialContent = mapper.Map<SerialContentAdminPageDto, SerialContent>(serialContentDto);
        CheckIfSubscriptionsHaveNewOne(serialContent.AllowedSubscriptions, await subscriptionRepository.GetAllSubscriptionsAsync());
        contentRepository.AddSerialContent(serialContent);
        await contentRepository.SaveChangesAsync();
        foreach (var season in serialContentDto.SeasonInfos)
        {
            foreach (var episode in season.Episodes)
            {
                if (episode.VideoFile != null)
                {
                    await PutSerialContentVideoAsync(serialContent.Id, episode.Resolution,
                        season.SeasonNumber, episode.EpisodeNumber, episode.VideoFile);
                }
            }
        }
    }

    public async Task<List<SectionDto>> GetSectionsAsync()
    {
        var result = new List<SectionDto>();
        
        var contentTypes = await contentTypeRepository.GetContentTypesAsync();
        foreach (var contentType in contentTypes)
        {
            var contents = await contentRepository
                .GetContentsByFilterWithAmountAsync(x => x.ContentTypeId == contentType.Id, 20);
            
            if (contents.Count <= 0) continue;
            
            result.Add(new SectionDto
            {
                Name = contentType.ContentTypeName,
                Contents = mapper.Map<List<SectionContentDto>>(contents)
            });
        }

        return result;
    }

    public async Task<List<PromoDto>> GetPromosAsync()
    {
        var contents = await contentRepository.GetRandomContentsAsync(5);
        return mapper.Map<List<PromoDto>>(contents);
    }

    public async Task<List<ContentTypeDto>> GetContentTypesAsync()
    {
        var contentTypes = await contentTypeRepository.GetContentTypesAsync();
        return mapper.Map<List<ContentTypeDto>>(contentTypes);
    }

    public async Task<List<GenreDto>> GetGenresAsync()
    {
        var genres = await genreRepository.GetGenresAsync();
        return mapper.Map<List<GenreDto>>(genres);
    }

    private void CheckIfSubscriptionsHaveNewOne(List<Subscription> subscriptions, List<Subscription> dbSubscriptions)
    {
        foreach (var subscription in subscriptions)
        {
            if (!dbSubscriptions.Any(dbs => dbs.Name.Equals(subscription.Name)))
            {
                throw new Exception("нельзя добавить свою подписку");
            }
        }
    }
    private Expression<Func<ContentBase, bool>> IsContentNameContain(Filter filter) =>
        content => filter.Name == null || content.Name.ToLower().Contains(filter.Name.ToLower());

    private Expression<Func<ContentBase, bool>> IsContentTypesContain(Filter filter) =>
        content => filter.Types == null || filter.Types.Count == 0 ||
                   filter.Types.Any(id => id == content.ContentTypeId);

    private Expression<Func<ContentBase, bool>> IsCountryContain(Filter filter) =>
        content => filter.Country == null ||
                   (content.Country != null && content.Country.ToLower() == filter.Country.ToLower());

    private Expression<Func<ContentBase, bool>> IsContentGenresContains(Filter filter) =>
        content => filter.Genres == null || filter.Genres.Count == 0 ||
                   filter.Genres.All(id => content.Genres.FirstOrDefault(g => g.Id == id) != null);

    private Expression<Func<ContentBase, bool>> IsContentYearBetween(Filter filter) =>
        content => content is MovieContent
            ?
            (!filter.ReleaseYearFrom.HasValue ||
             filter.ReleaseYearFrom.Value <= ((MovieContent)content).ReleaseDate.Year) &&
            (!filter.ReleaseYearTo.HasValue || filter.ReleaseYearTo.Value >= ((MovieContent)content).ReleaseDate.Year)
            : content is SerialContent
                ? (!filter.ReleaseYearFrom.HasValue ||
                   filter.ReleaseYearFrom.Value <= ((SerialContent)content).YearRange.Start.Year) &&
                  (!filter.ReleaseYearTo.HasValue ||
                   filter.ReleaseYearTo.Value >= ((SerialContent)content).YearRange.End.Year)
                :
                true;

    private Expression<Func<ContentBase, bool>> IsContentRatingBetween(Filter filter) =>
        content =>
            (filter.RatingFrom == null || filter.RatingFrom.Value <=
                (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating)) &&
            (filter.RatingTo == null ||
             filter.RatingTo.Value >= (content.Ratings == null ? 0 : content.Ratings.KinopoiskRating));
    
}