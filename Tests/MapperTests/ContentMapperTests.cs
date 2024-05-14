using Application.Dto;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Profiles;
using Tests.Customizations;
using Xunit.Abstractions;

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
        _fixture.Customizations.Add(new FormFileSpecimenBuilder());
        _testOutputHelper = testOutcomeHelper;
    }

    [Fact]
    public void MapSerialContentAdminPageDtoToSerialContent_MapsCorrectlyEachField()
    {
        // Arrange
        var serialContentAdminPageDto = _fixture.Build<SerialContentAdminPageDto>().Create();
        
        // Act
        var mappedContent = _mapper.Map<SerialContentAdminPageDto, SerialContent>(serialContentAdminPageDto);
        
        Assert.Equal(serialContentAdminPageDto.Name, mappedContent!.Name);
        Assert.Equal(serialContentAdminPageDto.Description, mappedContent.Description);
        Assert.Equal(serialContentAdminPageDto.Slogan, mappedContent.Slogan);
        Assert.Equal(serialContentAdminPageDto.PosterUrl, mappedContent.PosterUrl);
        Assert.Equal(serialContentAdminPageDto.Country, mappedContent.Country);


        Assert.Equal(serialContentAdminPageDto.ContentType, mappedContent.ContentType.ContentTypeName);
        Assert.Equal(serialContentAdminPageDto.AgeRating?.Age, mappedContent.AgeRatings?.Age);
        Assert.Equal(serialContentAdminPageDto.AgeRating?.AgeMpaa, mappedContent.AgeRatings?.AgeMpaa);
        Assert.Equal(serialContentAdminPageDto.Ratings?.KinopoiskRating, mappedContent.Ratings?.KinopoiskRating);
        Assert.Equal(serialContentAdminPageDto.Ratings?.ImdbRating, mappedContent.Ratings?.ImdbRating);
        Assert.Equal(serialContentAdminPageDto.Ratings?.LocalRating, mappedContent.Ratings?.LocalRating);
        Assert.Equal(serialContentAdminPageDto.TrailerInfo?.Url, mappedContent.TrailerInfo?.Url);
        Assert.Equal(serialContentAdminPageDto.TrailerInfo?.Name, mappedContent.TrailerInfo?.Name);
        Assert.Equal(serialContentAdminPageDto.Budget?.BudgetValue, mappedContent.Budget?.BudgetValue);
        Assert.Equal(serialContentAdminPageDto.Budget?.BudgetCurrencyName, mappedContent.Budget?.BudgetCurrencyName);


        Assert.Equal(serialContentAdminPageDto.AllowedSubscriptions.Count, mappedContent.AllowedSubscriptions.Count);
        Assert.True(serialContentAdminPageDto.AllowedSubscriptions.All(dtoSub =>
            mappedContent.AllowedSubscriptions.Any(sub =>
                sub.Name == dtoSub.Name && sub.Description == dtoSub.Description &&
                sub.MaxResolution == dtoSub.MaxResolution && dtoSub.Id == sub.Id
                && sub.Price.Equals(0))));

        Assert.Equal(serialContentAdminPageDto.Genres.Count, mappedContent.Genres.Count);
        Assert.True(serialContentAdminPageDto.Genres.All(name =>
            mappedContent.Genres.Any(genre => genre.Name == name)));

        Assert.Equal(serialContentAdminPageDto.PersonsInContent.Count, mappedContent.PersonsInContent.Count);
        Assert.True(serialContentAdminPageDto.PersonsInContent.All(dto =>
            mappedContent.PersonsInContent.Any(person =>
                person.Name == dto.Name && person.Profession.ProfessionName == dto.Profession)));

        Assert.Null(mappedContent.Reviews);
        
        Assert.Equal(serialContentAdminPageDto.SeasonInfos.Count, mappedContent.SeasonInfos.Count);
        foreach (var season in serialContentAdminPageDto.SeasonInfos)
        {
            var mappedSeason = mappedContent.SeasonInfos.FirstOrDefault(s => s.SeasonNumber == season.SeasonNumber);
            Assert.NotNull(mappedSeason);
            Assert.Equal(mappedSeason.SeasonNumber,season.SeasonNumber);

            foreach (var episode in mappedSeason.Episodes)
            {
                var mappedEpisode = mappedSeason.Episodes.FirstOrDefault(e => e.EpisodeNumber == episode.EpisodeNumber);
                Assert.NotNull(mappedEpisode);
                Assert.Equal(mappedEpisode.VideoUrl,episode.VideoUrl);
                Assert.Equal(mappedEpisode.Id,episode.Id);
            }
        }
    }
    [Fact]
    public void MapSerialContentToSerialContentAdminPageDto_MapsCorrectlyEachField()
    {
        // Arrange
        var serialContent = _fixture.Build<SerialContent>().Create();
        
        // Act
        var mappedContent = _mapper.Map<SerialContent, SerialContentAdminPageDto>(serialContent);
        
        // Assert
        Assert.Equal(serialContent.Name, mappedContent!.Name);
        Assert.Equal(serialContent.Description, mappedContent.Description);
        Assert.Equal(serialContent.Slogan, mappedContent.Slogan);
        Assert.Equal(serialContent.PosterUrl, mappedContent.PosterUrl);
        Assert.Equal(serialContent.Country, mappedContent.Country);


        Assert.Equal(serialContent.ContentType.ContentTypeName, mappedContent.ContentType);
        Assert.Equal(serialContent.AgeRatings?.Age, mappedContent.AgeRating?.Age);
        Assert.Equal(serialContent.AgeRatings?.AgeMpaa, mappedContent.AgeRating?.AgeMpaa);
        Assert.Equal(serialContent.Ratings?.KinopoiskRating, mappedContent.Ratings?.KinopoiskRating);
        Assert.Equal(serialContent.Ratings?.ImdbRating, mappedContent.Ratings?.ImdbRating);
        Assert.Equal(serialContent.Ratings?.LocalRating, mappedContent.Ratings?.LocalRating);
        Assert.Equal(serialContent.TrailerInfo?.Url, mappedContent.TrailerInfo?.Url);
        Assert.Equal(serialContent.TrailerInfo?.Name, mappedContent.TrailerInfo?.Name);
        Assert.Equal(serialContent.Budget?.BudgetValue, mappedContent.Budget?.BudgetValue);
        Assert.Equal(serialContent.Budget?.BudgetCurrencyName, mappedContent.Budget?.BudgetCurrencyName);


        Assert.Equal(serialContent.AllowedSubscriptions.Count, mappedContent.AllowedSubscriptions.Count);
        Assert.True(serialContent.AllowedSubscriptions.All(dtoSub =>
            mappedContent.AllowedSubscriptions.Any(sub =>
                sub.Name == dtoSub.Name && sub.Description == dtoSub.Description &&
                sub.MaxResolution == dtoSub.MaxResolution && dtoSub.Id == sub.Id)));

        Assert.Equal(serialContent.Genres.Count, mappedContent.Genres.Count);
        Assert.True(serialContent.Genres.All(genre =>
            mappedContent.Genres.Any(name => genre.Name == name)));

        Assert.Equal(serialContent.PersonsInContent.Count, mappedContent.PersonsInContent.Count);
        Assert.True(serialContent.PersonsInContent.All(pmc =>
            mappedContent.PersonsInContent.Any(pdmc =>
                pmc.Name == pdmc.Name && pmc.Profession.ProfessionName == pdmc.Profession)));
        
        Assert.Equal(serialContent.YearRange.Start, mappedContent.ReleaseYears.Start);
        Assert.Equal(serialContent.YearRange.End, mappedContent.ReleaseYears.End);
        
        Assert.Equal(serialContent.SeasonInfos.Count, mappedContent.SeasonInfos.Count);
        foreach (var season in serialContent.SeasonInfos)
        {
            var mappedSeason = mappedContent.SeasonInfos.FirstOrDefault(s => s.SeasonNumber == season.SeasonNumber);
            Assert.NotNull(mappedSeason);
            Assert.Equal(mappedSeason.SeasonNumber,season.SeasonNumber);

            foreach (var episode in mappedSeason.Episodes)
            {
                var mappedEpisode = mappedSeason.Episodes.FirstOrDefault(e => e.EpisodeNumber == episode.EpisodeNumber);
                Assert.NotNull(mappedEpisode);
                Assert.Equal(mappedEpisode.VideoUrl,episode.VideoUrl);
                Assert.Equal(mappedEpisode.Resolution,episode.Resolution);
                Assert.Equal(mappedEpisode.VideoFile?.Length, episode.VideoFile?.Length);
                Assert.Equal(mappedEpisode.VideoFile?.ContentType, episode.VideoFile?.ContentType);
                Assert.Equal(mappedEpisode.VideoFile?.Name, episode.VideoFile?.Name);
                Assert.Equal(mappedEpisode.VideoFile?.ContentDisposition, episode.VideoFile?.ContentDisposition);
                Assert.Equal(mappedEpisode.VideoFile?.Headers, episode.VideoFile?.Headers);
            }
        }
    }
    [Fact]
    public void MapMovieContentToMovieContentAdminPageDto_MapsCorrectlyEachField()
    {
        // Arrange
        var movieContent = _fixture.Build<MovieContent>().Create();
        
        // Act
        var mappedContent = _mapper.Map<MovieContent, MovieContentAdminPageDto>(movieContent);
        
        // Assert
        Assert.Equal(movieContent.Name, mappedContent!.Name);
        Assert.Equal(movieContent.Description, mappedContent.Description);
        Assert.Equal(movieContent.Slogan, mappedContent.Slogan);
        Assert.Equal(movieContent.PosterUrl, mappedContent.PosterUrl);
        Assert.Equal(movieContent.Country, mappedContent.Country);
        Assert.Equal(movieContent.MovieLength, mappedContent.MovieLength);
        Assert.Equal(movieContent.VideoUrl, mappedContent.VideoUrl);
        Assert.Equal(movieContent.ReleaseDate, mappedContent.ReleaseDate);


        Assert.Equal(movieContent.ContentType.ContentTypeName, mappedContent.ContentType);
        Assert.Equal(movieContent.AgeRatings?.Age, mappedContent.AgeRatings?.Age);
        Assert.Equal(movieContent.AgeRatings?.AgeMpaa, mappedContent.AgeRatings?.AgeMpaa);
        Assert.Equal(movieContent.Ratings?.KinopoiskRating, mappedContent.Ratings?.KinopoiskRating);
        Assert.Equal(movieContent.Ratings?.ImdbRating, mappedContent.Ratings?.ImdbRating);
        Assert.Equal(movieContent.Ratings?.LocalRating, mappedContent.Ratings?.LocalRating);
        Assert.Equal(movieContent.TrailerInfo?.Url, mappedContent.TrailerInfo?.Url);
        Assert.Equal(movieContent.TrailerInfo?.Name, mappedContent.TrailerInfo?.Name);
        Assert.Equal(movieContent.Budget?.BudgetValue, mappedContent.Budget?.BudgetValue);
        Assert.Equal(movieContent.Budget?.BudgetCurrencyName, mappedContent.Budget?.BudgetCurrencyName);


        Assert.Equal(movieContent.AllowedSubscriptions.Count, mappedContent.AllowedSubscriptions.Count);
        Assert.True(movieContent.AllowedSubscriptions.All(dtoSub =>
            mappedContent.AllowedSubscriptions.Any(sub =>
                sub.Name == dtoSub.Name && sub.Description == dtoSub.Description &&
                sub.MaxResolution == dtoSub.MaxResolution && dtoSub.Id == sub.Id)));

        Assert.Equal(movieContent.Genres.Count, mappedContent.Genres.Count);
        Assert.True(movieContent.Genres.All(genre =>
            mappedContent.Genres.Any(name => genre.Name == name)));

        Assert.Equal(movieContent.PersonsInContent.Count, mappedContent.PersonsInContent.Count);
        Assert.True(movieContent.PersonsInContent.All(pmc =>
            mappedContent.PersonsInContent.Any(pdmc =>
                pmc.Name == pdmc.Name && pmc.Profession.ProfessionName == pdmc.Profession)));
    }
    [Fact]
    public void MapMovieContentAdminPageDtoToMovieContent_MapsCorrectlyEachField()
    {
        // Arrange
        var mcDto = _fixture.Build<MovieContentAdminPageDto>().Create();

        // Act
        var mappedContent = _mapper.Map<MovieContentAdminPageDto, MovieContent>(mcDto);

        // Assert

        Assert.Equal(mcDto.Name, mappedContent!.Name);
        Assert.Equal(mcDto.Description, mappedContent.Description);
        Assert.Equal(mcDto.Slogan, mappedContent.Slogan);
        Assert.Equal(mcDto.PosterUrl, mappedContent.PosterUrl);
        Assert.Equal(mcDto.Country, mappedContent.Country);
        Assert.Equal(mcDto.MovieLength, mappedContent.MovieLength);
        Assert.Equal(mcDto.VideoUrl, mappedContent.VideoUrl);
        Assert.Equal(mcDto.ReleaseDate, mappedContent.ReleaseDate);


        Assert.Equal(mcDto.ContentType, mappedContent.ContentType.ContentTypeName);
        Assert.Equal(mcDto.AgeRatings?.Age, mappedContent.AgeRatings?.Age);
        Assert.Equal(mcDto.AgeRatings?.AgeMpaa, mappedContent.AgeRatings?.AgeMpaa);
        Assert.Equal(mcDto.Ratings?.KinopoiskRating, mappedContent.Ratings?.KinopoiskRating);
        Assert.Equal(mcDto.Ratings?.ImdbRating, mappedContent.Ratings?.ImdbRating);
        Assert.Equal(mcDto.Ratings?.LocalRating, mappedContent.Ratings?.LocalRating);
        Assert.Equal(mcDto.TrailerInfo?.Url, mappedContent.TrailerInfo?.Url);
        Assert.Equal(mcDto.TrailerInfo?.Name, mappedContent.TrailerInfo?.Name);
        Assert.Equal(mcDto.Budget?.BudgetValue, mappedContent.Budget?.BudgetValue);
        Assert.Equal(mcDto.Budget?.BudgetCurrencyName, mappedContent.Budget?.BudgetCurrencyName);


        Assert.Equal(mcDto.AllowedSubscriptions.Count, mappedContent.AllowedSubscriptions.Count);
        Assert.True(mcDto.AllowedSubscriptions.All(dtoSub =>
            mappedContent.AllowedSubscriptions.Any(sub =>
                sub.Name == dtoSub.Name && sub.Description == dtoSub.Description &&
                sub.MaxResolution == dtoSub.MaxResolution && dtoSub.Id == sub.Id
                && sub.Price.Equals(0))));

        Assert.Equal(mcDto.Genres.Count, mappedContent.Genres.Count);
        Assert.True(mcDto.Genres.All(name =>
            mappedContent.Genres.Any(genre => genre.Name == name)));

        Assert.Equal(mcDto.PersonsInContent.Count, mappedContent.PersonsInContent.Count);
        Assert.True(mcDto.PersonsInContent.All(dto =>
            mappedContent.PersonsInContent.Any(person =>
                person.Name == dto.Name && person.Profession.ProfessionName == dto.Profession)));

        Assert.Null(mappedContent.Reviews);
    }
}