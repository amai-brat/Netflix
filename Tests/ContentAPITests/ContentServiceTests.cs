﻿using AutoFixture;
using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services;
using Domain.Services.ServiceExceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ContentAPITests
{
    public class ContentServiceTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IContentRepository> _mockContent = new();

        [Fact]
        public async Task GetExistedContentByIdShouldReturnContent()
        {
            //Arrange
            var availableContent = BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();
            var id = availableContent[Random.Shared.Next(0, availableContent.Count)].Id;

            //Act
            _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var result = await service.GetContentByIdAsync(id);

            //Assert
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public async Task GetNotExistedContentByIdShouldReturnNull()
        {
            //Arrange
            var availableContent = BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();
            var id = -1;

            //Act
            _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var result = await service.GetContentByIdAsync(id);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(Filters))]
        public async Task GetContentByNotDefaultFilterShouldReturnFilteredContent(Filter filter)
        {
            //Arrange
            var filteredContent = BuildFilteredMovieContentBaseList(filter).Concat(BuildFilteredSerialContentBaseList(filter));
            var unfilteredContent = BuildUnFilteredMovieContentBaseList(filter).Concat(BuildUnFilteredSerialContentBaseList(filter));
            var availableContent = filteredContent.Concat(unfilteredContent).ToArray();
            Random.Shared.Shuffle(availableContent);

            //Act
            _mockContent.Setup(repository => repository.GetContentsByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.Where(filter.Compile()).ToList());

            var service = new ContentService(_mockContent.Object);
            var result = await service.GetContentsByFilterAsync(filter);

            //Assert
            Assert.True(result.All(filteredContent.Contains));
        }

        [Fact]
        public async Task GetContentByDefaultFilterShouldReturnAllContent()
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseList().Concat(BuildDefaultSerialContentBaseList()).ToList();

            //Act
            _mockContent.Setup(repository => repository.GetContentsByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => contents.Where(filter.Compile()).ToList());

            var service = new ContentService(_mockContent.Object);
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
            _mockContent.Setup(repository => repository.GetMovieContentByFilterAsync(It.IsAny<Expression<Func<MovieContent, bool>>>()))
                .ReturnsAsync((Expression<Func<MovieContent, bool>> filter) => contents.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
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
            contents.First(x => x.Id == contentId).SeasonInfos[season - 1].Episodes[episode - 1].VideoUrl = "a/resolution/a";

            //Act
            _mockContent.Setup(repository => repository.GetSerialContentByFilterAsync(It.IsAny<Expression<Func<SerialContent, bool>>>()))
                .ReturnsAsync((Expression<Func<SerialContent, bool>> filter) => contents.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var result = await service.GetSerialContentVideoUrlAsync(contentId, season, episode, resolution, subId);

            //Assert
            Assert.Equal($"a/{resolution}/a", result);
        }

        [Theory]
        [InlineData(400, 1, ErrorMessages.NotFoundResolution)]
        [InlineData(480, -1, ErrorMessages.NotFoundContent)]
        public async Task GetMovieContentUrlWithInCorrectDataShouldThrowArgException(int resolution, int contentId, string errorMsg)
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseListWithAllowedSub().Cast<MovieContent>().ToList();
            var _contentId = contentId == -1 ? contentId : contents[Random.Shared.Next(0, contents.Count)].Id;
            var subId = contentId == -1 ? -1 : contents.First(x => x.Id == _contentId).AllowedSubscriptions.First().Id;

            //Act
            _mockContent.Setup(repository => repository.GetMovieContentByFilterAsync(It.IsAny<Expression<Func<MovieContent, bool>>>()))
                .ReturnsAsync((Expression<Func<MovieContent, bool>> filter) => contents.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var ex = await Assert.ThrowsAsync<ContentServiceArgumentException>(async () => { await service.GetMovieContentVideoUrlAsync(_contentId, resolution, subId); });

            //Assert
            Assert.Contains(errorMsg, ex.Message);
        }

        [Theory]
        [InlineData(400, 1, 1, 2, ErrorMessages.NotFoundResolution)]
        [InlineData(480, -1, 1, 2, ErrorMessages.NotFoundContent)]
        [InlineData(480, 1, -1, 2, ErrorMessages.NotFoundSeason)]
        [InlineData(480, 1, 1, -1, ErrorMessages.NotFoundEpisode)]
        public async Task GetSerialContentUrlWithInCorrectDataShouldThrowArgException(int resolution, long contentId, int season, int episode, string errorMsg)
        {
            //Arrange
            var contents = BuildDefaultSerialContentBaseListWithAllowedSub().Cast<SerialContent>().ToList();
            var _contentId = contentId == -1 ? contentId : contents[Random.Shared.Next(0, contents.Count)].Id;
            var subId = contentId == -1 ? -1 : contents.First(x => x.Id == _contentId).AllowedSubscriptions.First().Id;
            var _season = season == -1 ? -1 : 1;
            var _episode = episode == -1 ? episode : 2;

            //Act
            _mockContent.Setup(repository => repository.GetSerialContentByFilterAsync(It.IsAny<Expression<Func<SerialContent, bool>>>()))
                .ReturnsAsync((Expression<Func<SerialContent, bool>> filter) => contents.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var ex = await Assert.ThrowsAsync<ContentServiceArgumentException>(async () => { await service.GetSerialContentVideoUrlAsync(_contentId,_season, _episode, resolution, subId); });

            //Assert
            Assert.Contains(errorMsg, ex.Message);
        }

        [Theory]
        [InlineData(480, -1)]
        [InlineData(2160, 1)]
        public async Task GetMovieContentUrlWithInCorrectSubIdOrResShouldThrowNotPermittedException(int resolution, int subId)
        {
            //Arrange
            var contents = BuildDefaultMovieContentBaseListWithAllowedSub().Cast<MovieContent>().ToList();
            var contentId = contents[Random.Shared.Next(0, contents.Count)].Id;
            contents.First(x => x.Id == contentId).AllowedSubscriptions.First().MaxResolution = 1080;
            var _subId = subId == -1 ? subId : contents.First(x => x.Id == contentId).AllowedSubscriptions.First().Id;
            var _resolution = resolution;

            //Act
            _mockContent.Setup(repository => repository.GetMovieContentByFilterAsync(It.IsAny<Expression<Func<MovieContent, bool>>>()))
                .ReturnsAsync((Expression<Func<MovieContent, bool>> filter) => contents.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var ex = await Assert.ThrowsAsync<ContentServiceNotPermittedException>(async () => { await service.GetMovieContentVideoUrlAsync(contentId, _resolution, _subId); });

            //Assert
            Assert.Contains(ErrorMessages.UserDoesNotHavePermissionBySubscription, ex.Message);
        }

        [Theory]
        [InlineData(480, -1)]
        [InlineData(2160, 1)]
        public async Task GetSerialContentUrlWithInCorrectSubIdOrResShouldThrowNotPermittedException(int resolution, int subId)
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
            _mockContent.Setup(repository => repository.GetSerialContentByFilterAsync(It.IsAny<Expression<Func<SerialContent, bool>>>()))
                .ReturnsAsync((Expression<Func<SerialContent, bool>> filter) => contents.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var ex = await Assert.ThrowsAsync<ContentServiceNotPermittedException>(async () => { await service.GetSerialContentVideoUrlAsync(contentId, season, episode, _resolution, _subId); });

            //Assert
            Assert.Contains(ErrorMessages.UserDoesNotHavePermissionBySubscription, ex.Message);
        }

        public static IEnumerable<object[]> Filters()
        {
            var dataList = new List<object[]>()
            {
                new object[]{new Filter() {
                    Name = "d"
                }},
                new object[]{new Filter() {
                    Types = [3, 4]
                }},
                new object[]{new Filter() {
                    Genres = [1, 3]
                }},
                new object[]{new Filter() {
                    Country = "USA"
                }},
                new object[]{new Filter() {
                    ReleaseYearFrom = 2014
                }},
                new object[]{new Filter() {
                    ReleaseYearTo = 2020
                }},
                new object[]{new Filter() {
                    RatingFrom = 5
                }},
                new object[]{new Filter() {
                    RatingTo = 9
                }}
            };
            foreach(var data in dataList)
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
            serials.ForEach(s => {
                s.AllowedSubscriptions = BuildDefaultAllowedSub();
                s.SeasonInfos = [ 
                    new SeasonInfo(){
                        SeasonNumber = 1,
                        Episodes = [ new Episode() { EpisodeNumber = 1}, new Episode() { EpisodeNumber = 2}]
                    },
                    new SeasonInfo(){
                        SeasonNumber = 2,
                        Episodes = [ new Episode() { EpisodeNumber = 1}, new Episode() { EpisodeNumber = 2}]
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
            var contents = BuildDefaultMovieContentBaseList().Cast<MovieContent>();

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
            var contents = BuildDefaultMovieContentBaseList().Cast<MovieContent>();

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
            var contents = BuildDefaultSerialContentBaseList().Cast<SerialContent>();
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
                    content.YearRange = new YearRange() { End = DateOnly.MinValue};
                if (filter.RatingFrom is not null)
                    content.Ratings!.KinopoiskRating = 10;
                if (filter.RatingTo is not null)
                    content.Ratings!.KinopoiskRating = 0;
            }
            return contents.Cast<ContentBase>().ToList();
        }

        private List<ContentBase> BuildUnFilteredSerialContentBaseList(Filter filter)
        {
            var contents = BuildDefaultSerialContentBaseList().Cast<SerialContent>();

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
                    content.YearRange= new YearRange() { End = DateOnly.MaxValue };
                if (filter.RatingFrom is not null)
                    content.Ratings!.KinopoiskRating = -1;
                if (filter.RatingTo is not null)
                    content.Ratings!.KinopoiskRating = 11;
            }
            return contents.Cast<ContentBase>().ToList();
        }
    }
}