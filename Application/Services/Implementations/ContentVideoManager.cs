using System.Diagnostics;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Implementations;

public class ContentVideoManager(
    IContentRepository contentRepository,
    IContentVideoProvider contentVideoProvider)
{
    private readonly HashSet<int> _resolutions = [360, 480, 720, 1080, 1440, 2160];

    public async Task<string> GetMovieContentM3U8UrlAsync(long movieId, int resolution)
    {
        var resource = await GetMovieVideoUrlAsync(movieId, resolution)  + ".m3u8";
        return await contentVideoProvider.GetUrlAsync(resource);
    }

    public async Task<string> GetSerialContentM3U8UrlAsync(long serialId, int seasonNumber, int episodeNumber, int resolution)
    {
        var serialUrl = await GetSerialEpisodeVideoUrlAsync(serialId, resolution, seasonNumber, episodeNumber) + ".m3u8";
        return await contentVideoProvider.GetUrlAsync(serialUrl);
    }

    public async Task<string> GetMovieContentStreamUrlAsync(long movieId, int resolution)
    {
        var resource = await GetMovieVideoUrlAsync(movieId, resolution)  + ".ts";
        return await contentVideoProvider.GetUrlAsync(resource);
    }

    public async Task<string> GetSerialContentStreamUrlAsync(long serialId, int seasonNumber, int episodeNumber, int resolution)
    {
        var serialUrl = await GetSerialEpisodeVideoUrlAsync(serialId, resolution, seasonNumber, episodeNumber) + ".ts";
        return await contentVideoProvider.GetUrlAsync(serialUrl);
    }
    
    public async Task PutMovieContentVideoAsync(long movieId, int resolution, IFormFile videoStream)
    {
        // сохраняет видео из формы временно на сервере
        var tempFilePath = await SaveVideoToTempFile(videoStream);
        
        // берем сохраненное видео и генерируем из него .m3u8 и .ts файлы, возвращает путь к этим файлам
        var pathToGeneratedHlsFiles = await ConvertMovieFileToHls(tempFilePath, movieId, resolution);
        
        // эти файлы сохраняем в minio
        await SaveMovieHlsFilesToMinio(pathToGeneratedHlsFiles, movieId, resolution);
    }
    
    public async Task PutSerialContentVideoAsync(long serialId, int resolution, int seasonNumber,int episodeNumber,IFormFile videoStream)
    {
        // сохраняет видео из формы временно на сервере
        var tempFilePath = await SaveVideoToTempFile(videoStream);
        
        // берем сохраненное видео и генерируем из него .m3u8 и .ts файлы, возвращает путь к этим файлам
        var pathToGeneratedHlsFiles = await ConvertSerialFileToHls(tempFilePath,serialId,resolution,seasonNumber,episodeNumber);
        
        // эти файлы сохраняем в minio
        await SaveSerialHlsFilesToMinio(pathToGeneratedHlsFiles,serialId,resolution,seasonNumber,episodeNumber);
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
            throw new ArgumentValidationException("HLS_BASE_URL env variable not set", "");
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
            throw new ArgumentValidationException("HLS_BASE_URL env variable not set", "");
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

    private async Task SaveSerialHlsFilesToMinio(string pathToFiles, long id, int resolution, int seasonNumber,
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
    
    private async Task<string> GetMovieVideoUrlAsync(long movieId, int resolution)
    {
        var movie = await contentRepository.GetMovieContentByFilterAsync(m => m.Id == movieId);
        if (movie is null)
            throw new ArgumentValidationException(ErrorMessages.NotFoundContent, $"{movieId}");
        if (!_resolutions.Contains(resolution))
            throw new ArgumentValidationException(ErrorMessages.NotFoundResolution, $"{resolution}");

        string url = movie.VideoUrl;
        url = url.Replace("{res}", resolution.ToString());
        url = url.Replace("{id}", movieId.ToString());

        return url;
    }

    private async Task<string> GetSerialEpisodeVideoUrlAsync(long serialId, int resolution, int seasonNumber, int episodeNumber)
    {
        var serial = await contentRepository.GetSerialContentByFilterAsync(s => s.Id == serialId);
        if (serial is null)
            throw new ArgumentValidationException(ErrorMessages.NotFoundContent, $"{serialId}");

        if (!_resolutions.Contains(resolution))
            throw new ArgumentValidationException(ErrorMessages.NotFoundResolution, $"{resolution}");

        if (!serial.SeasonInfos.Select(s => s.SeasonNumber)
                .Contains(seasonNumber))
            throw new ArgumentValidationException(ErrorMessages.NotFoundSeason, $"{seasonNumber}");

        if (!serial.SeasonInfos.Single(s => s.SeasonNumber == seasonNumber).Episodes
                .Select(e => e.EpisodeNumber)
                .Contains(episodeNumber))
            throw new ArgumentValidationException(ErrorMessages.NotFoundEpisode, $"{episodeNumber}");

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
}