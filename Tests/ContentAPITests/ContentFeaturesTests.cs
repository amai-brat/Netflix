using System.Linq.Expressions;
using Application.Features.Contents.Commands.AddMovieContent;
using Application.Features.Contents.Commands.AddSerialContent;
using Application.Features.Contents.Commands.DeleteContent;
using Application.Features.Contents.Commands.UpdateMovieContent;
using Application.Features.Contents.Commands.UpdateSerialContent;
using Application.Features.Contents.Dtos;
using Application.Features.Contents.Queries.GetContentsByFilter;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoFixture;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests.Customizations;

namespace Tests.ContentAPITests;

public class ContentFeaturesTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IContentRepository> _mockContent = new();
    private readonly Mock<IContentVideoManager> _mockContentVideoManager = new();
    private readonly Mock<ISubscriptionRepository> _mockSubscription = new();
    private readonly Mock<IContentTypeRepository> _mockContentType = new();
    private readonly Mock<IGenreRepository> _mockGenre = new();
    private readonly Mock<IUserRepository> _mockUser = new();
    private readonly Mock<IContentVideoProvider> _mockContentVideoProvider = new();
    private readonly IServiceProvider _serviceProvider;

    public ContentFeaturesTests()
    {
        _serviceProvider = new TestServiceProviderBuilder()
            .With(services =>
            {
                services.AddScoped<IContentRepository>(_ => _mockContent.Object);
                services.AddScoped<ISubscriptionRepository>(_ => _mockSubscription.Object);
                services.AddScoped<IContentTypeRepository>(_ => _mockContentType.Object);
                services.AddScoped<IUserRepository>(_ => _mockUser.Object);
                services.AddScoped<IContentVideoProvider>(_ => _mockContentVideoProvider.Object);
                services.AddScoped<IGenreRepository>(_ => _mockGenre.Object);
                services.AddScoped<IContentVideoManager>(_ => _mockContentVideoManager.Object);
                
                services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            })
            .Build();
        
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
        _fixture.Customizations.Add(new FormFileSpecimenBuilder());
    }
    
    [Fact]
    public async Task AddMovieContent_WithValidData_ShouldAddMovieContent()
    {
        // Arrange
        var subscriptions = GetDefaultSubscriptions();
        var movieContent = BuildValidMovie(subscriptions);
        var dataSource = new List<MovieContent>();
        
        _mockContent.Setup(repo => repo.AddMovieContent(It.IsAny<MovieContent>()))
            .Callback<MovieContent>(mc => dataSource.Add(mc));
        _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
            .ReturnsAsync(subscriptions);
        var mediator = _serviceProvider.GetService<IMediator>()!;

        
        // Act
        await mediator.Send(new AddMovieContentCommand(movieContent));
        
        // Assert
        Assert.Single(dataSource);
    }

    [Fact]
    public async Task AddSerialContent_ShouldAddContent_Unit()
    {
        // Arrange
        var subscriptions = GetDefaultSubscriptions();
        var serialContent = BuildValidSerial(subscriptions);
        //
        var dataSource = new List<SerialContent>();
        
        _mockContent.Setup(repo => repo.AddSerialContent(It.IsAny<SerialContent>()))
            .Callback<SerialContent>(mc => dataSource.Add(mc));
        _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
            .ReturnsAsync(subscriptions);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // Act
        await mediator.Send(new AddSerialContentCommand(serialContent));
        
        // Assert
        Assert.Single(dataSource);
    }

    [Fact]
    public async Task DeleteContent_WithGivenId_ShouldDeleteContent()
    {
        // Arrange
        var contentId = 1;
        var dataSource = new List<ContentBase>()
        {
            new() {Id = 1}
        };
        _mockContent.Setup(repo => repo.DeleteContent(contentId))
            .Callback<long>(id => dataSource.RemoveAll(c => c.Id == id))
            .Returns(new ContentBase());
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // Act
        await mediator.Send(new DeleteContentCommand(contentId));
        
        // Assert
        Assert.Empty(dataSource);
    }

    [Fact]
    public async Task UpdateMovieContent_WithValidData_ShouldUpdateMovieContent()
    {
        // Arrange
        var subscriptions = GetDefaultSubscriptions();
        var movieContent = BuildValidMovie(subscriptions);
        var dataSource = new List<MovieContent>();
        
        _mockContent.Setup(repo => repo.UpdateMovieContent(It.IsAny<MovieContent>()))
            .Callback<MovieContent>(mc => dataSource.Add(mc));
        _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
            .ReturnsAsync(subscriptions);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // Act
        await mediator.Send(new UpdateMovieContentCommand(movieContent));
        
        // Assert
        Assert.Single(dataSource);
        
    }

    [Fact]
    public async Task UpdateSerialContent_WithValidData_ShouldUpdateSerialContent()
    {
        // Arrange
        var subscriptions = GetDefaultSubscriptions();
        var serialContent = BuildValidSerial(subscriptions);
        var dataSource = new List<SerialContent>();
        
        _mockContent.Setup(repo => repo.UpdateSerialContent(It.IsAny<SerialContent>()))
            .Callback<SerialContent>(mc => dataSource.Add(mc));
        _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
            .ReturnsAsync(subscriptions);
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // Act
        await mediator.Send(new UpdateSerialContentCommand(serialContent));
        
        // Assert
        Assert.Single(dataSource);
    }

    [Theory]
    [MemberData(nameof(Filters))]
    public async Task GetContentByNotDefaultFilterShouldReturnFilteredContent(Filter filter)
    {
        //Arrange
        var filteredContent = BuildFilteredMovieContentBaseList(filter)
            .Concat(BuildFilteredSerialContentBaseList(filter));
        var unfilteredContent = BuildUnFilteredMovieContentBaseList(filter)
            .Concat(BuildUnFilteredSerialContentBaseList(filter));
        var contentBases = filteredContent.ToList();
        var availableContent = contentBases.Concat(unfilteredContent).ToArray();
        Random.Shared.Shuffle(availableContent);
        var mediator = _serviceProvider.GetService<IMediator>()!;

        //Act
        _mockContent.Setup(repository =>
                repository.GetContentsByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
            .ReturnsAsync((Expression<Func<ContentBase, bool>> f) =>
                availableContent.Where(f.Compile()).ToList());
        
        var result = await mediator.Send(new GetContentsByFilterQuery(filter));

        //Assert
        Assert.True(result.Contents.All(contentBases.Contains));
    }

    [Fact]
    public async Task GetContentByDefaultFilterShouldReturnAllContent()
    {
        //Arrange
        var contents = BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();
        var mediator = _serviceProvider.GetService<IMediator>()!;

        //Act
        _mockContent.Setup(repository =>
                repository.GetContentsByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
            .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) =>
                contents.Where(filter.Compile()).ToList());
        
        var result = await mediator.Send(new GetContentsByFilterQuery(new Filter()));

        //Assert
        Assert.True(result.Contents.All(contents.Contains));
    }

    public static IEnumerable<object[]> Filters()
    {
        var dataList = new List<object[]>()
        {
            new object[]
            {
                new Filter()
                {
                    Name = "d"
                }
            },
            new object[]
            {
                new Filter()
                {
                    Types = [3, 4]
                }
            },
            new object[]
            {
                new Filter()
                {
                    Genres = [1, 3]
                }
            },
            new object[]
            {
                new Filter()
                {
                    Country = "USA"
                }
            },
            new object[]
            {
                new Filter()
                {
                    ReleaseYearFrom = 2014
                }
            },
            new object[]
            {
                new Filter()
                {
                    ReleaseYearTo = 2020
                }
            },
            new object[]
            {
                new Filter()
                {
                    RatingFrom = 5
                }
            },
            new object[]
            {
                new Filter()
                {
                    RatingTo = 9
                }
            }
        };
        foreach (var data in dataList)
            yield return data;
    }

    private List<ContentBase> BuildDefaultMovieContentBaseList() =>
        _fixture.Build<MovieContent>()
            .Without(c => c.PersonsInContent)
            .Without(c => c.AllowedSubscriptions)
            .Without(c => c.ContentType)
            .Without(c => c.Genres)
            .Without(c => c.Reviews)
            .Without(c => c.ReleaseDate)
            .Do(c => { c.Id = Math.Abs(c.Id); })
            .CreateMany(20)
            .Cast<ContentBase>().ToList();

    private List<ContentBase> BuildDefaultSerialContentBaseList() =>
        _fixture.Build<SerialContent>()
            .Without(c => c.PersonsInContent)
            .Without(c => c.AllowedSubscriptions)
            .Without(c => c.ContentType)
            .Without(c => c.Genres)
            .Without(c => c.Reviews)
            .Without(c => c.YearRange)
            .Without(c => c.SeasonInfos)
            .Do(c => { c.Id = Math.Abs(c.Id); })
            .CreateMany(20)
            .Cast<ContentBase>().ToList();

    private List<ContentBase> BuildFilteredMovieContentBaseList(Filter filter)
    {
        var contents = BuildDefaultMovieContentBaseList().Cast<MovieContent>().ToList();

        foreach (var content in contents)
        {
            if (filter.Name is not null)
                content.Name += filter.Name;
            if (filter.Types is not null)
                content.ContentTypeId = filter.Types[Random.Shared.Next(0, filter.Types.Count)];
            if (filter.Genres is not null)
                content.Genres = filter.Genres.Select(g => new Genre() { Id = g }).ToList();
            if (filter.Country is not null)
                content.Country = filter.Country;
            if (filter.ReleaseYearFrom is not null)
                content.ReleaseDate = DateOnly.MaxValue;
            if (filter.ReleaseYearTo is not null)
                content.ReleaseDate = DateOnly.MinValue;
            if (filter.RatingFrom is not null)
                content.Ratings!.KinopoiskRating = 10;
            if (filter.RatingTo is not null)
                content.Ratings!.KinopoiskRating = 0;
        }

        return contents.Cast<ContentBase>().ToList();
    }

    private List<ContentBase> BuildUnFilteredMovieContentBaseList(Filter filter)
    {
        var contents = BuildDefaultMovieContentBaseList().Cast<MovieContent>().ToList();

        foreach (var content in contents)
        {
            if (filter.Name is not null)
                content.Name = string.Empty;
            if (filter.Types is not null)
                content.ContentTypeId = filter.Types.Sum();
            if (filter.Genres is not null)
                content.Genres = [];
            if (filter.Country is not null)
                content.Country = string.Empty;
            if (filter.ReleaseYearFrom is not null)
                content.ReleaseDate = DateOnly.MinValue;
            if (filter.ReleaseYearTo is not null)
                content.ReleaseDate = DateOnly.MaxValue;
            if (filter.RatingFrom is not null)
                content.Ratings!.KinopoiskRating = -1;
            if (filter.RatingTo is not null)
                content.Ratings!.KinopoiskRating = 11;
        }

        return contents.Cast<ContentBase>().ToList();
    }

    private List<ContentBase> BuildFilteredSerialContentBaseList(Filter filter)
    {
        var contents = BuildDefaultSerialContentBaseList().Cast<SerialContent>().ToList();
        foreach (var content in contents)
        {
            if (filter.Name is not null)
                content.Name += filter.Name;
            if (filter.Types is not null)
                content.ContentTypeId = filter.Types[Random.Shared.Next(0, filter.Types.Count)];
            if (filter.Genres is not null)
                content.Genres = filter.Genres.Select(g => new Genre() { Id = g }).ToList();
            if (filter.Country is not null)
                content.Country = filter.Country;
            if (filter.ReleaseYearFrom is not null)
                content.YearRange = new YearRange() { Start = DateOnly.MaxValue };
            if (filter.ReleaseYearTo is not null)
                content.YearRange = new YearRange() { End = DateOnly.MinValue };
            if (filter.RatingFrom is not null)
                content.Ratings!.KinopoiskRating = 10;
            if (filter.RatingTo is not null)
                content.Ratings!.KinopoiskRating = 0;
        }

        return contents.Cast<ContentBase>().ToList();
    }

    private List<ContentBase> BuildUnFilteredSerialContentBaseList(Filter filter)
    {
        var contents = BuildDefaultSerialContentBaseList().Cast<SerialContent>().ToList();

        foreach (var content in contents)
        {
            if (filter.Name is not null)
                content.Name = string.Empty;
            if (filter.Types is not null)
                content.ContentTypeId = filter.Types.Sum();
            if (filter.Genres is not null)
                content.Genres = [];
            if (filter.Country is not null)
                content.Country = string.Empty;
            if (filter.ReleaseYearFrom is not null)
                content.YearRange = new YearRange() { Start = DateOnly.MinValue };
            if (filter.ReleaseYearTo is not null)
                content.YearRange = new YearRange() { End = DateOnly.MaxValue };
            if (filter.RatingFrom is not null)
                content.Ratings!.KinopoiskRating = -1;
            if (filter.RatingTo is not null)
                content.Ratings!.KinopoiskRating = 11;
        }

        return contents.Cast<ContentBase>().ToList();
    }

    private MovieContentDto BuildValidMovie(List<Subscription> subscriptions)
    {
        return _fixture.Build<MovieContentDto>()
            .With(dto => dto.AllowedSubscriptions,
                subscriptions.Select(s =>
                    new SubscriptionDto
                    {
                        Name = s.Name,
                        Description = s.Description,
                        Id = s.Id,
                        MaxResolution = s.MaxResolution
                    }).ToList)
            .OmitAutoProperties()
            .With(x => x.Name, "The Rise and Fall (3L)")
            .With(x => x.ContentType, "Movie")
            .With(x => x.Description, Guid.NewGuid().ToString())
            .With(x => x.PosterUrl, Guid.NewGuid().ToString())
            .With(x => x.MovieLength, 228)
            .With(x => x.ReleaseDate, DateOnly.FromDateTime(DateTime.Now.AddDays(-1)))
            .With(x => x.VideoUrl, Guid.NewGuid().ToString())
            .With(x => x.Genres, ["Touhou"])
            .With(x => x.PersonsInContent, [new PersonInContentDto{Name = "John Titor", Profession = "Traveler"}])
            .Create();
    }

    private SerialContentDto BuildValidSerial(List<Subscription> subscriptions)
    {
        return _fixture.Build<SerialContentDto>()
            .With(dto => dto.AllowedSubscriptions,
                subscriptions.Select(s =>
                    new SubscriptionDto
                    {
                        Name = s.Name,
                        Description = s.Description,
                        Id = s.Id,
                        MaxResolution = s.MaxResolution
                    }).ToList)
            .OmitAutoProperties()
            .With(x => x.Name, "The Rise and Fall (3L)")
            .With(x => x.ContentType, "Movie")
            .With(x => x.Description, Guid.NewGuid().ToString())
            .With(x => x.PosterUrl, Guid.NewGuid().ToString())
            .With(x => x.Genres, ["Touhou"])
            .With(x => x.PersonsInContent, [new PersonInContentDto{Name = "John Titor", Profession = "Traveler"}])
            .With(x => x.SeasonInfos, [new SeasonInfoDto
            {
                SeasonNumber = 1,
                Episodes = [new EpisodeDto
                {
                    EpisodeNumber = 1,
                    VideoUrl = Guid.NewGuid().ToString()
                }]
            }])
            .Create();
    }

    private List<Subscription> GetDefaultSubscriptions()
    {
        return _fixture.CreateMany<Subscription>().ToList();
    }
}