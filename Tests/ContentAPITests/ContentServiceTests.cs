using AutoFixture;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;
using Application.Dto;
using Application.Exceptions.ErrorMessages;
using Application.Exceptions.Particular;
using Application.Repositories;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using AutoMapper;
using FluentValidation;
using Infrastructure.Profiles;
using Tests.Customizations;
using Xunit.Abstractions;
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Local

namespace Tests.ContentAPITests
{
    public class ContentServiceTests
    {
        private ITestOutputHelper _testOutputHelper;
        private Fixture _fixture = new();
        private Mock<IContentRepository> _mockContent = new();
        private Mock<ISubscriptionRepository> _mockSubscription = new();
        private Mock<IContentTypeRepository> _mockContentType = new();
        private Mock<IGenreRepository> _mockGenre = new();
        private Mock<IUserRepository> _mockUser = new();
        private Mock<IValidator<MovieContentAdminPageDto>> _mockMovieContentValidator = new();
        private Mock<IValidator<SerialContentAdminPageDto>> _mockSerialContentValidator = new();
        private Mock<IContentVideoProvider> _mockContentVideoProvider = new();
        private IMapper _mapper;

        public ContentServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new ContentProfile()); });
            _mapper = mapperConfig.CreateMapper();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
            _fixture.Customizations.Add(new FormFileSpecimenBuilder());
        }
        
        [Fact]
        public async Task GetMovieContentM3U8UrlAsync_WithNonExistentContent_ShouldThrow()
        {
            // Arrange
            ContentService service = GetService();
            var user = new User
            {
                UserSubscriptions =
                [
                    new UserSubscription() { SubscriptionId = 1 }
                ]
            };

            _mockContent.Setup(cr => cr.GetContentWithAllowedSubscriptionsByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);
            _mockUser.Setup(repo => repo.GetUserWithSubscriptionsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            
            // Act
            Task<string> Act() => service.GetMovieContentM3U8UrlAsync(1, 1, 480);

            // Assert
            await Assert.ThrowsAsync<ContentServiceNotPermittedException>(Act);
        }
        [Fact]
        public async Task GetMovieContentM3U8UrlAsync_WithoutSubscription_ShouldThrow()
        {
            // Arrange
            ContentService service = GetService();
            _mockContent.Setup(cr => cr.GetContentWithAllowedSubscriptionsByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(null,TimeSpan.FromMilliseconds(1));
            _mockUser.Setup(repo => repo.GetUserWithSubscriptionsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));
             
            // Act
             Task<string> Act() => service.GetMovieContentM3U8UrlAsync(1,1,480);
             
            // Assert
            await Assert.ThrowsAsync<ContentServiceNotPermittedException>(Act);
        }
        [Fact]
        public async Task GetMovieContentM3U8UrlAsync_WithIncorrectResolution_ShouldThrow()
        {
            // Arrange
            ContentService service = GetService();
            var user = new User
            {
                UserSubscriptions =
                [
                    new UserSubscription() { SubscriptionId = 1 }
                ]
            };
            var content = new MovieContent
            {
                AllowedSubscriptions = [new Subscription() { Id = 1 }]
            };
            _mockContent.Setup(cr => cr.GetContentWithAllowedSubscriptionsByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(content);
            _mockUser.Setup(repo => repo.GetUserWithSubscriptionsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);
            // Act
            Task<string> Act() => service.GetMovieContentM3U8UrlAsync(1,1,228);
             
            // Assert
            await Assert.ThrowsAsync<ContentServiceArgumentException>(Act);
        }
        [Fact]
        public async Task AddMovieContent_WithValidData_ShouldAddMovieContent()
        {
            // Arrange
            var subscriptions = GetDefaultSubscriptions();
            var movieContent = _fixture.Build<MovieContentAdminPageDto>()
                .With(dto => dto.AllowedSubscriptions,
                    subscriptions.Select(s =>
                        new SubscriptionAdminPageDto
                        {
                            Name = s.Name,
                            Description = s.Description,
                            Id = s.Id,
                            MaxResolution = s.MaxResolution
                        }).ToList)
                .OmitAutoProperties()
                .Create();
            var dataSource = new List<MovieContent>();
            
            _mockContent.Setup(repo => repo.AddMovieContent(It.IsAny<MovieContent>()))
                .Callback<MovieContent>(mc => dataSource.Add(mc));
            _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
                .ReturnsAsync(subscriptions);
            _mockMovieContentValidator.Setup(v => v.Validate(It.IsAny<MovieContentAdminPageDto>()))
                .Returns(new FluentValidation.Results.ValidationResult());
            var contentService = GetService();
            
            // Act
            await contentService.AddMovieContent(movieContent);
            
            // Assert
            Assert.Single(dataSource);
        }

        [Fact]
        public async Task AddSerialContent_ShouldAddContent_Unit()
        {
            // Arrange
            var subscriptions = GetDefaultSubscriptions();
            var movieContent = _fixture.Build<SerialContentAdminPageDto>()
                
                .With(dto => dto.AllowedSubscriptions,
                    subscriptions.Select(s =>
                        new SubscriptionAdminPageDto
                        {
                            Name = s.Name,
                            Description = s.Description,
                            Id = s.Id,
                            MaxResolution = s.MaxResolution
                        }).ToList)
                .OmitAutoProperties()
                .Create();
            //
            var dataSource = new List<SerialContent>();
            
            _mockContent.Setup(repo => repo.AddSerialContent(It.IsAny<SerialContent>()))
                .Callback<SerialContent>(mc => dataSource.Add(mc));
            _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
                .ReturnsAsync(subscriptions);
            _mockSerialContentValidator.Setup(v => v.Validate(It.IsAny<SerialContentAdminPageDto>()))
                .Returns(new FluentValidation.Results.ValidationResult());
            var contentService = GetService();
            
            // Act
            await contentService.AddSerialContent(movieContent);
            
            // Assert
            Assert.Single(dataSource);
        }

        [Fact]
        public async Task DeleteContent_WithGivenId_ShouldDeleteContent()
        {
            // Arrange
            var contentId = 1;
            var dataSource = new List<ContentBase>();
            _mockContent.Setup(repo => repo.DeleteContent(contentId))
                .Callback<long>(id => dataSource.RemoveAll(c => c.Id == id))
                .Returns(new ContentBase());
            var contentService = GetService();
            
            // Act
            await contentService.DeleteContent(contentId);
            
            // Assert
            Assert.Empty(dataSource);
        }

        [Fact]
        public async Task UpdateMovieContent_WithValidData_ShouldUpdateMovieContent()
        {
            // Arrange
            var subscriptions = GetDefaultSubscriptions();
            var movieContent = _fixture.Build<MovieContentAdminPageDto>()
                .With(dto => dto.AllowedSubscriptions,
                    subscriptions.Select(s =>
                        new SubscriptionAdminPageDto
                        {
                            Name = s.Name,
                            Description = s.Description,
                            Id = s.Id,
                            MaxResolution = s.MaxResolution
                        }).ToList)
                .OmitAutoProperties()
                .Create();
            var dataSource = new List<MovieContent>();
            
            _mockContent.Setup(repo => repo.UpdateMovieContent(It.IsAny<MovieContent>()))
                .Callback<MovieContent>(mc => dataSource.Add(mc));
            _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
                .ReturnsAsync(subscriptions);
            _mockMovieContentValidator.Setup(v => v.Validate(It.IsAny<MovieContentAdminPageDto>()))
                .Returns(new FluentValidation.Results.ValidationResult());
            var contentService = GetService();
            
            // Act
            await contentService.UpdateMovieContent(movieContent);
            
            // Assert
            Assert.Single(dataSource);
            
        }

        [Fact]
        public async Task UpdateSerialContent_WithValidData_ShouldUpdateSerialContent()
        {
            // Arrange
            var subscriptions = GetDefaultSubscriptions();
            var movieContent = _fixture.Build<SerialContentAdminPageDto>()
                .With(dto => dto.AllowedSubscriptions,
                    subscriptions.Select(s =>
                        new SubscriptionAdminPageDto
                        {
                            Name = s.Name,
                            Description = s.Description,
                            Id = s.Id,
                            MaxResolution = s.MaxResolution
                        }).ToList)
                .OmitAutoProperties()
                .Create();
            var dataSource = new List<SerialContent>();
            
            _mockContent.Setup(repo => repo.UpdateSerialContent(It.IsAny<SerialContent>()))
                .Callback<SerialContent>(mc => dataSource.Add(mc));
            _mockSubscription.Setup(repo => repo.GetAllSubscriptionsAsync())
                .ReturnsAsync(subscriptions);
            _mockSerialContentValidator.Setup(v => v.Validate(It.IsAny<SerialContentAdminPageDto>()))
                .Returns(new FluentValidation.Results.ValidationResult());
            var contentService = GetService();
            
            // Act
            await contentService.UpdateSerialContent(movieContent);
            
            // Assert
            Assert.Single(dataSource);
        }

        [Fact]
        public async Task GetExistedContentByIdShouldReturnContent()
        {
            //Arrange
            var availableContent =
                BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();
            var id = availableContent[Random.Shared.Next(0, availableContent.Count)].Id;

            //Act
            _mockContent.Setup(repository =>
                    repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) =>
                    availableContent.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var result = await service.GetContentByIdAsync(id);

            //Assert
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public async Task GetNotExistedContentByIdShouldReturnNull()
        {
            //Arrange
            var availableContent =
                BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();
            var id = -1;

            //Act
            _mockContent.Setup(repository =>
                    repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) =>
                    availableContent.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var result = await service.GetContentByIdAsync(id);

            //Assert
            Assert.Null(result);
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

            //Act
            _mockContent.Setup(repository =>
                    repository.GetContentsByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> f) =>
                    availableContent.Where(f.Compile()).ToList());

            var service = GetService();
            var result = await service.GetContentsByFilterAsync(filter);

            //Assert
            Assert.True(result.All(contentBases.Contains));
        }

        [Fact]
        public async Task GetContentByDefaultFilterShouldReturnAllContent()
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();

            //Act
            _mockContent.Setup(repository =>
                    repository.GetContentsByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) =>
                    contents.Where(filter.Compile()).ToList());

            var service = GetService();
            var result = await service.GetContentsByFilterAsync(new Filter());

            //Assert
            Assert.True(result.All(contents.Contains));
        }

        [Theory]
        [InlineData(480)]
        [InlineData(720)]
        [InlineData(1080)]
        [InlineData(1440)]
        [InlineData(2160)]
        public async Task GetMovieContentUrlWithCorrectDataShouldReturnCorrectUrl(int resolution)
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseListWithAllowedSub().Cast<MovieContent>().ToList();
            var contentId = contents[Random.Shared.Next(0, contents.Count)].Id;
            var subId = contents.First(x => x.Id == contentId).AllowedSubscriptions.First().Id;
            contents.First(x => x.Id == contentId).VideoUrl = "a/resolution/a";

            //Act
            _mockContent.Setup(repository =>
                    repository.GetMovieContentByFilterAsync(It.IsAny<Expression<Func<MovieContent, bool>>>()))
                .ReturnsAsync((Expression<Func<MovieContent, bool>> filter) =>
                    contents.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var result = await service.GetMovieContentVideoUrlAsync(contentId, resolution, subId);

            //Assert
            Assert.Equal($"a/{resolution}/a", result);
        }

        [Theory]
        [InlineData(480)]
        [InlineData(720)]
        [InlineData(1080)]
        [InlineData(1440)]
        [InlineData(2160)]
        public async Task GetSerialContentUrlWithCorrectDataShouldReturnCorrectUrl(int resolution)
        {
            //Arrange
            var contents = BuildDefaultSerialContentBaseListWithAllowedSub().Cast<SerialContent>().ToList();
            var contentId = contents[Random.Shared.Next(0, contents.Count)].Id;
            var subId = contents.First(x => x.Id == contentId).AllowedSubscriptions.First().Id;
            var season = 1;
            var episode = 2;
            contents.First(x => x.Id == contentId).SeasonInfos[season - 1].Episodes[episode - 1].VideoUrl =
                "a/resolution/a";

            //Act
            _mockContent.Setup(repository =>
                    repository.GetSerialContentByFilterAsync(It.IsAny<Expression<Func<SerialContent, bool>>>()))
                .ReturnsAsync((Expression<Func<SerialContent, bool>> filter) =>
                    contents.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var result = await service.GetSerialContentVideoUrlAsync(contentId, season, episode, resolution, subId);

            //Assert
            Assert.Equal($"a/{resolution}/a", result);
        }

        [Theory]
        [InlineData(400, 1, ErrorMessages.NotFoundResolution)]
        [InlineData(480, -1, ErrorMessages.NotFoundContent)]
        public async Task GetMovieContentUrlWithInCorrectDataShouldThrowArgException(int resolution, int contentId,
            string errorMsg)
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseListWithAllowedSub().Cast<MovieContent>().ToList();
            var _contentId = contentId == -1 ? contentId : contents[Random.Shared.Next(0, contents.Count)].Id;
            var subId = contentId == -1 ? -1 : contents.First(x => x.Id == _contentId).AllowedSubscriptions.First().Id;

            //Act
            _mockContent.Setup(repository =>
                    repository.GetMovieContentByFilterAsync(It.IsAny<Expression<Func<MovieContent, bool>>>()))
                .ReturnsAsync((Expression<Func<MovieContent, bool>> filter) =>
                    contents.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ContentServiceArgumentException>(async () =>
            {
                await service.GetMovieContentVideoUrlAsync(_contentId, resolution, subId);
            });

            //Assert
            Assert.Contains(errorMsg, ex.Message);
        }

        [Theory]
        [InlineData(400, 1, 1, 2, ErrorMessages.NotFoundResolution)]
        [InlineData(480, -1, 1, 2, ErrorMessages.NotFoundContent)]
        [InlineData(480, 1, -1, 2, ErrorMessages.NotFoundSeason)]
        [InlineData(480, 1, 1, -1, ErrorMessages.NotFoundEpisode)]
        public async Task GetSerialContentUrlWithInCorrectDataShouldThrowArgException(int resolution, long contentId,
            int season, int episode, string errorMsg)
        {
            //Arrange
            var contents = BuildDefaultSerialContentBaseListWithAllowedSub().Cast<SerialContent>().ToList();
            var _contentId = contentId == -1 ? contentId : contents[Random.Shared.Next(0, contents.Count)].Id;
            var subId = contentId == -1 ? -1 : contents.First(x => x.Id == _contentId).AllowedSubscriptions.First().Id;
            var _season = season == -1 ? -1 : 1;
            var _episode = episode == -1 ? episode : 2;

            //Act
            _mockContent.Setup(repository =>
                    repository.GetSerialContentByFilterAsync(It.IsAny<Expression<Func<SerialContent, bool>>>()))
                .ReturnsAsync((Expression<Func<SerialContent, bool>> filter) =>
                    contents.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ContentServiceArgumentException>(async () =>
            {
                await service.GetSerialContentVideoUrlAsync(_contentId, _season, _episode, resolution, subId);
            });

            //Assert
            Assert.Contains(errorMsg, ex.Message);
        }

        [Theory]
        [InlineData(480, -1)]
        [InlineData(2160, 1)]
        public async Task GetMovieContentUrlWithInCorrectSubIdOrResShouldThrowNotPermittedException(int resolution,
            int subId)
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseListWithAllowedSub().Cast<MovieContent>().ToList();
            var contentId = contents[Random.Shared.Next(0, contents.Count)].Id;
            contents.First(x => x.Id == contentId).AllowedSubscriptions.First().MaxResolution = 1080;
            var _subId = subId == -1 ? subId : contents.First(x => x.Id == contentId).AllowedSubscriptions.First().Id;
            var _resolution = resolution;

            //Act
            _mockContent.Setup(repository =>
                    repository.GetMovieContentByFilterAsync(It.IsAny<Expression<Func<MovieContent, bool>>>()))
                .ReturnsAsync((Expression<Func<MovieContent, bool>> filter) =>
                    contents.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ContentServiceNotPermittedException>(async () =>
            {
                await service.GetMovieContentVideoUrlAsync(contentId, _resolution, _subId);
            });

            //Assert
            Assert.Contains(ErrorMessages.UserDoesNotHavePermissionBySubscription, ex.Message);
        }

        [Theory]
        [InlineData(480, -1)]
        [InlineData(2160, 1)]
        public async Task GetSerialContentUrlWithInCorrectSubIdOrResShouldThrowNotPermittedException(int resolution,
            int subId)
        {
            //Arrange
            var contents = BuildDefaultSerialContentBaseListWithAllowedSub().Cast<SerialContent>().ToList();
            var contentId = contents[Random.Shared.Next(0, contents.Count)].Id;
            contents.First(x => x.Id == contentId).AllowedSubscriptions.First().MaxResolution = 1080;
            var _subId = subId == -1 ? subId : contents.First(x => x.Id == contentId).AllowedSubscriptions.First().Id;
            var season = 1;
            var episode = 2;
            var _resolution = resolution;

            //Act
            _mockContent.Setup(repository =>
                    repository.GetSerialContentByFilterAsync(It.IsAny<Expression<Func<SerialContent, bool>>>()))
                .ReturnsAsync((Expression<Func<SerialContent, bool>> filter) =>
                    contents.SingleOrDefault(filter.Compile()));

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ContentServiceNotPermittedException>(async () =>
            {
                await service.GetSerialContentVideoUrlAsync(contentId, season, episode, _resolution, _subId);
            });

            //Assert
            Assert.Contains(ErrorMessages.UserDoesNotHavePermissionBySubscription, ex.Message);
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

        private List<ContentBase> BuildDefaultMovieContentBaseListWithAllowedSub()
        {
            var movies = _fixture.Build<MovieContent>()
                .Without(c => c.PersonsInContent)
                .Without(c => c.AllowedSubscriptions)
                .Without(c => c.ContentType)
                .Without(c => c.Genres)
                .Without(c => c.Reviews)
                .Without(c => c.ReleaseDate)
                .Do(c => { c.Id = Math.Abs(c.Id); })
                .CreateMany(20)
                .Cast<ContentBase>().ToList();
            movies.ForEach(m => m.AllowedSubscriptions = BuildDefaultAllowedSub());
            return movies;
        }

        private List<ContentBase> BuildDefaultSerialContentBaseListWithAllowedSub()
        {
            var serials = _fixture.Build<SerialContent>()
                .Without(c => c.PersonsInContent)
                .Without(c => c.AllowedSubscriptions)
                .Without(c => c.ContentType)
                .Without(c => c.Genres)
                .Without(c => c.Reviews)
                .Without(c => c.YearRange)
                .Without(c => c.SeasonInfos)
                .Do(c => { c.Id = Math.Abs(c.Id); })
                .CreateMany(20)
                .ToList();
            serials.ForEach(s =>
            {
                s.AllowedSubscriptions = BuildDefaultAllowedSub();
                s.SeasonInfos =
                [
                    new SeasonInfo()
                    {
                        SeasonNumber = 1,
                        Episodes = [new Episode() { EpisodeNumber = 1 }, new Episode() { EpisodeNumber = 2 }]
                    },
                    new SeasonInfo()
                    {
                        SeasonNumber = 2,
                        Episodes = [new Episode() { EpisodeNumber = 1 }, new Episode() { EpisodeNumber = 2 }]
                    }
                ];
            });
            return serials.Cast<ContentBase>().ToList();
        }

        private List<Subscription> BuildDefaultAllowedSub()
        {
            var allowed = _fixture.Build<Subscription>()
                .Without(s => s.AccessibleContent)
                .CreateMany(10)
                .ToList();
            allowed.ForEach(a => { a.MaxResolution = 2160; });
            return allowed;
        }

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

        private ContentService GetService()
        {
            return new ContentService(_mockContent.Object,
                _mockSubscription.Object,
                _mockContentType.Object,
                _mockGenre.Object,
                _mockContentVideoProvider.Object,
                _mockUser.Object,
                _mockMovieContentValidator.Object,
                _mockSerialContentValidator.Object,
                _mapper);
        }

        private List<Subscription> GetDefaultSubscriptions()
        {
            return _fixture.CreateMany<Subscription>().ToList();
        }
    }
}
