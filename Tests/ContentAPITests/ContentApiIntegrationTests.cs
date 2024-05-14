using System.Text;
using System.Text.Json;
using Application.Dto;
using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests.ContentAPITests;

public class ContentApiIntegrationTests(WebAppFactory factory, ITestOutputHelper output) : IClassFixture<WebAppFactory>
{
    
    [Fact]
    public async Task AddMovieContent_CorrectContentGiven_ContentAdded()
    {
        // Arrange
        var client = factory.CreateAdminHttpClient();
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "content/movie/add");
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"Name", "Test movie"},
            {"Description", "Test description"},
            {"ReleaseDate", DateOnly.FromDateTime(DateTime.Now).ToString()},
            {"Slogan", "Test slogan"},
            {"Genres[0]", "Action"},
            {"AllowedSubscriptions[0].Name", "Сериалы"},
            {"ContentType", "Сериал"},
            {"MovieLength", "120"},
            {"PosterUrl", "123"},
            {"VideoUrl", "123"},
            {"PersonsInContent[0].Name", "123"},
            {"PersonsInContent[0].Profession", "123"}
        });
        postRequest.Content = content;
        
        // Act
        var response = await client.SendAsync(postRequest);
        
        // Assert
        response.EnsureSuccessStatusCode();
        // check if data is added to the database
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.NotNull(context.MovieContents.FirstOrDefault(movie => movie.Name == "Test movie"));
    }
    [Fact]
    public async Task AddSerialContent_CorrectContentGiven_ContentAdded()
    {
        // Arrange
        var client = factory.CreateAdminHttpClient();
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "content/serial/add");
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"Name", "Test series"},
            {"Description", "Test description"},
            {"ReleaseDate", DateOnly.FromDateTime(DateTime.Now).ToString()},
            {"Slogan", "Test slogan"},
            {"Genres[0]", "Action"},
            {"AllowedSubscriptions[0].Name", "Сериалы"},
            {"ContentType", "Сериал"},
            {"PosterUrl", "123"},
            {"ReleaseYears.Start",DateOnly.FromDateTime(DateTime.Now).ToString()},
            {"ReleaseYears.End", DateOnly.FromDateTime(DateTime.Now.AddDays(1)).ToString()},
            {"SeasonInfos[0].SeasonNumber", "1"},
            {"SeasonInfos[0].Episodes[0].EpisodeNumber", "123"},
            {"SeasonInfos[0].Episodes[0].VideoUrl", "123"},
            {"SeasonInfos[0].Episodes[0].Resolution", "123"},
            {"PersonsInContent[0].Name", "123"},
            {"PersonsInContent[0].Profession", "123"}
        });
        postRequest.Content = content;
        
        // Act
        var response = await client.SendAsync(postRequest);
        output.WriteLine(await response.Content.ReadAsStringAsync());
        // Assert
        response.EnsureSuccessStatusCode();
        // check if data is added to the database
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.NotNull(context.SerialContents.FirstOrDefault(series => series.Name == "Test series"));
    }
}