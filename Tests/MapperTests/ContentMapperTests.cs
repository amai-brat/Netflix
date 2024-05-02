using Application.Dto;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Profiles;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using Tests.SpecimenBuilders;
using Xunit.Abstractions;
using Xunit.Sdk;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.MapperTests;

public class ContentMapperTests
{
    private Fixture _fixture = new();

    private IMapper _mapper;
    private ITestOutputHelper _testOutputHelper;
    public ContentMapperTests(ITestOutputHelper testOutcomeHelper)
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new ContentProfile()); });
        _mapper = mapperConfig.CreateMapper();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
        _testOutputHelper = testOutcomeHelper;
    }

    [Fact]
    public async Task MapMovieContentAdminPageDtoToSerialContent()
    {
        // Arrange
        var dto = _fixture.Build<MovieContentAdminPageDto>().Create();
        
        // Act
        var savedContent = _mapper.Map<MovieContentAdminPageDto, MovieContent>(dto);
        
        // Assert
        
        Assert.Equal(dto.Name, savedContent!.Name);
            Assert.Equal(dto.Description, savedContent.Description);
            Assert.Equal(dto.Slogan, savedContent.Slogan);
            Assert.Equal(dto.PosterUrl, savedContent.PosterUrl);
            Assert.Equal(dto.Country, savedContent.Country);
            Assert.Equal(dto.MovieLength,savedContent.MovieLength);
            Assert.Equal(dto.VideoUrl, savedContent.VideoUrl);
            Assert.Equal(dto.ReleaseDate, savedContent.ReleaseDate);
            
            
            Assert.Equal(dto.ContentType, savedContent.ContentType.ContentTypeName);
            Assert.Equal(dto.AgeRatings?.Age, savedContent.AgeRatings?.Age);
            Assert.Equal(dto.AgeRatings?.AgeMpaa, savedContent.AgeRatings?.AgeMpaa);
            Assert.Equal(dto.Ratings?.KinopoiskRating,savedContent.Ratings?.KinopoiskRating);
            Assert.Equal(dto.Ratings?.ImdbRating,savedContent.Ratings?.ImdbRating);
            Assert.Equal(dto.Ratings?.LocalRating,savedContent.Ratings?.LocalRating);
            Assert.Equal(dto.TrailerInfo?.Url,savedContent.TrailerInfo?.Url);
            Assert.Equal(dto.TrailerInfo?.Name,savedContent.TrailerInfo?.Name);
            Assert.Equal(dto.Budget?.BudgetValue,savedContent.Budget?.BudgetValue);
            Assert.Equal(dto.Budget?.BudgetCurrencyName,savedContent.Budget?.BudgetCurrencyName);
            
            
            Assert.Equal(dto.AllowedSubscriptions.Count, savedContent.AllowedSubscriptions.Count);
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(dto.AllowedSubscriptions));
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(savedContent.AllowedSubscriptions));
            Assert.True(dto.AllowedSubscriptions.All(dtoSub => 
                savedContent.AllowedSubscriptions.Any(sub => 
                    sub.Name == dtoSub.Name && sub.Description == dtoSub.Description && 
                    sub.MaxResolution == dtoSub.MaxResolution && dtoSub.Id == sub.Id
                    && sub.Price == 0)));
            
            Assert.Equal(dto.Genres.Count, savedContent.Genres.Count);
            Assert.True(dto.Genres.All(name => 
                savedContent.Genres.Any(genre => genre.Name == name)));
            
            Assert.Equal(dto.PersonsInContent.Count, savedContent.PersonsInContent.Count);
            Assert.True(dto.PersonsInContent.All(dto => 
                savedContent.PersonsInContent.Any(person => 
                    person.Name == dto.Name && person.Profession.ProfessionName == dto.Profession)));
            
            Assert.Null(savedContent.Reviews);
    }
}