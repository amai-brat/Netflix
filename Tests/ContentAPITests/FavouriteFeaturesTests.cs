using System.Linq.Expressions;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Favourites.Commands.AddFavourite;
using Application.Features.Favourites.Commands.RemoveFavourite;
using Application.Repositories;
using AutoFixture;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.ContentAPITests;

public class FavouriteFeaturesTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IContentRepository> _mockContent = new();
    private readonly Mock<IFavouriteContentRepository> _mockFav = new();
    private readonly Mock<IUserRepository> _mockUser = new();
    private readonly IServiceProvider _serviceProvider;

    public FavouriteFeaturesTests()
    {
        _serviceProvider = new TestServiceProviderBuilder()
            .With(services =>
            {
                services.AddScoped<IContentRepository>(_ => _mockContent.Object);
                services.AddScoped<IFavouriteContentRepository>(_ => _mockFav.Object);
                services.AddScoped<IUserRepository>(_ => _mockUser.Object);
            })
            .Build();
    }

    [Fact]
    public async Task AddToFavouriteWithCorrectArgsShouldWorkCorrect()
    {
        //Arrange
        var availableContent = BuildDefaultContentBaseList();
        var users = BuildDefaultUserList();
        var contentId = availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
        var userId = users[Random.Shared.Next(0, users.Count)].Id;
        var userFav = new List<FavouriteContent>();


        //Act
        _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
            .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
        _mockFav.Setup(repository => repository.GetFavouriteContentsByFilterAsync(It.IsAny<Expression<Func<FavouriteContent, bool>>>()))
            .ReturnsAsync((Expression<Func<FavouriteContent, bool>> filter) => userFav.Where(filter.Compile()).ToList());
        _mockFav.Setup(repository => repository.AddFavouriteContentAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Callback((long cId, long uId) => { userFav.Add(new FavouriteContent() { UserId = uId, ContentId = cId }); });

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        await mediator.Send(new AddFavouriteCommand(contentId, userId));

        //Assert
        Assert.Equal(userId, userFav[0].UserId);
        Assert.Equal(contentId, userFav[0].ContentId);
    }

    [Fact]
    public async Task RemoveFromFavouriteWithCorrectArgsShouldWorkCorrect()
    {
        //Arrange
        var availableContent = BuildDefaultContentBaseList();
        var users = BuildDefaultUserList();
        var contentId = availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
        var userId = users[Random.Shared.Next(0, users.Count)].Id;
        var userFav = new List<FavouriteContent>
        {
            new() { UserId = -1, ContentId = -1 },
            new() { UserId = userId, ContentId = contentId }
        };

        //Act
        _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
            .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
        _mockFav.Setup(repository => repository.GetFavouriteContentsByFilterAsync(It.IsAny<Expression<Func<FavouriteContent, bool>>>()))
            .ReturnsAsync((Expression<Func<FavouriteContent, bool>> filter) => userFav.Where(filter.Compile()).ToList());
        _mockFav.Setup(repository => repository.RemoveFavouriteContentAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Callback((long cId, long uId) => { userFav.Remove(userFav.First(f => f.UserId == uId && f.ContentId == cId)); });

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        await mediator.Send(new RemoveFavouriteCommand(contentId, userId));

        //Assert
        Assert.NotEqual(userId ,userFav[0].UserId);
        Assert.NotEqual(contentId, userFav[0].ContentId);
    }

    [Theory]
    [InlineData(-1, 0, ErrorMessages.NotFoundUser)]
    [InlineData(0, -1, ErrorMessages.NotFoundContent)]
    [InlineData(0, 0, ErrorMessages.AlreadyFavourite)]
    public async Task AddToFavouriteWithInCorrectArgsShouldThrowException(long userId, long contentId, string errorMsg)
    {
        //Arrange
        var availableContent = BuildDefaultContentBaseList();
        var users = BuildDefaultUserList();
        var contentId1 = contentId == -1 ? contentId : availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
        var userId1 = userId == -1 ? userId : users[Random.Shared.Next(0, users.Count)].Id;
        var userFav = new List<FavouriteContent>()
        {
            new(){UserId = userId1, ContentId = contentId1}
        };

        //Act
        _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
            .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
        _mockFav.Setup(repository => repository.GetFavouriteContentsByFilterAsync(It.IsAny<Expression<Func<FavouriteContent, bool>>>()))
            .ReturnsAsync((Expression<Func<FavouriteContent, bool>> filter) => userFav.Where(filter.Compile()).ToList());
        _mockFav.Setup(repository => repository.RemoveFavouriteContentAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Callback((long cId, long uId) => { userFav.Remove(userFav.First(f => f.UserId == uId && f.ContentId == cId)); });

        var mediator = _serviceProvider.GetService<IMediator>()!;
        var ex = await Assert.ThrowsAsync<ArgumentValidationException>(async () =>
        {
            await mediator.Send(new AddFavouriteCommand(contentId1, userId1));
        });

        //Assert
        Assert.Contains(errorMsg, ex.Message);
    }

    [Theory]
    [InlineData(-1, 0, ErrorMessages.NotFoundUser)]
    [InlineData(0, -1, ErrorMessages.NotFoundContent)]
    [InlineData(0, 0, ErrorMessages.NotInFavourite)]
    public async Task RemoveFromFavouriteWithInCorrectArgsShouldThrowException(long userId, long contentId, string errorMsg)
    {
        //Arrange
        var availableContent = BuildDefaultContentBaseList();
        var users = BuildDefaultUserList();
        var contentId1 = contentId == -1 ? contentId : availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
        var userId1 = userId == -1 ? userId : users[Random.Shared.Next(0, users.Count)].Id;
        var userFav = new List<FavouriteContent>()
        {
            new() {UserId = long.MaxValue, ContentId = long.MaxValue}
        };

        //Act
        _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
        _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
            .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
        _mockFav.Setup(repository => repository.GetFavouriteContentsByFilterAsync(It.IsAny<Expression<Func<FavouriteContent, bool>>>()))
            .ReturnsAsync((Expression<Func<FavouriteContent, bool>> filter) => userFav.Where(filter.Compile()).ToList());
        _mockFav.Setup(repository => repository.RemoveFavouriteContentAsync(It.IsAny<long>(), It.IsAny<long>()))
            .Callback((long cId, long uId) => { userFav.Remove(userFav.First(f => f.UserId == uId && f.ContentId == cId)); });

        var mediator = _serviceProvider.GetService<IMediator>()!;
        var ex = await Assert.ThrowsAsync<ArgumentValidationException>(async () =>
        {
            await mediator.Send(new RemoveFavouriteCommand(contentId1, userId1));
        });

        //Assert
        Assert.Contains(errorMsg, ex.Message);
    }
    private List<ContentBase> BuildDefaultContentBaseList() =>
        _fixture.Build<ContentBase>()
        .Without(c => c.PersonsInContent)
        .Without(c => c.AllowedSubscriptions)
        .Without(c => c.ContentType)
        .Without(c => c.Genres)
        .Without(c => c.Reviews)
        .Do(c => { c.Id = Math.Abs(c.Id); })
        .CreateMany(20)
        .ToList();

    private List<User> BuildDefaultUserList() =>
        _fixture.Build<User>()
        .Without(u => u.Reviews)
        .Without(u => u.ScoredComments)
        .Without(u => u.UserSubscriptions)
        .Without(u => u.FavouriteContents)
        .Without(u => u.Comments)
        .Without(u => u.ScoredReviews)
        .Without(u => u.BirthDay)
        .Do(u => { u.Id = Math.Abs(u.Id); })
        .CreateMany(20)
        .ToList();
}