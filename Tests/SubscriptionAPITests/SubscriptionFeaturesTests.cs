using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Subscriptions.Commands.CreateSubscription;
using Application.Features.Subscriptions.Commands.DeleteSubscription;
using Application.Features.Subscriptions.Commands.EditSubscription;
using Application.Repositories;
using AutoFixture;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.SubscriptionAPITests;

public class SubscriptionFeaturesTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ISubscriptionRepository> _mockSubRepo = new();
    private readonly Mock<IContentRepository> _mockContentRepo = new();
    private readonly IServiceProvider _serviceProvider;

    public SubscriptionFeaturesTests()
    {
        _serviceProvider = new TestServiceProviderBuilder()
            .With(services =>
            {
                services.AddScoped<ISubscriptionRepository>(_ => _mockSubRepo.Object);
                services.AddScoped<IContentRepository>(_ => _mockContentRepo.Object);
            })
            .Build();
    }

    [Fact]
    public async Task AddSubscription_CorrectDtoGiven_EntityAdded()
    {
        // arrange
        var contents = GetContents(10);
        var contentIds = contents.Select(x => x.Id).ToList();
        var command = _fixture.Build<CreateSubscriptionCommand>()
            .With(x => x.AccessibleContentIds, contentIds)
            .Create();
        var subscriptions = new List<Subscription>();
        
        _mockContentRepo.Setup(x => x.GetContentByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) => contents.FirstOrDefault(x => x.Id == id));
        _mockSubRepo
            .Setup(x => x.AddAsync(It.IsAny<Subscription>()))
            .Callback((Subscription x) => { subscriptions.Add(x); })
            .ReturnsAsync(()  => subscriptions.Last());
        
        var mediator = _serviceProvider.GetService<IMediator>()!;

        // act
        var subscription = await mediator.Send(command);
        
        // assert
        _mockSubRepo.Verify(x => x.AddAsync(It.IsAny<Subscription>()), Times.Once);
        Assert.Contains(subscription, subscriptions);
        foreach (var content in contents)
        {
            Assert.Contains(content, subscription.AccessibleContent);
        }
    }

    [Theory]
    [InlineData("", "a", 480, SubscriptionErrorMessages.NotValidSubscriptionName)]
    [InlineData("NETFLIX(!)", "a", 480, SubscriptionErrorMessages.NotValidSubscriptionName)]
    [InlineData("a", "", 480, SubscriptionErrorMessages.NotValidSubscriptionDescription)]
    [InlineData("a", "a", 0, SubscriptionErrorMessages.NotValidSubscriptionMaxResolution)]
    [InlineData("a", "a", -1080, SubscriptionErrorMessages.NotValidSubscriptionMaxResolution)]
    public async Task AddSubscription_IncorrectDtoGiven_ExceptionThrown(string name, string desc, int maxResolution, string errorMsg)
    {
        // arrange
        var command = _fixture.Build<CreateSubscriptionCommand>()
            .With(x => x.Name, name)
            .With(x => x.Description, desc)
            .With(x => x.MaxResolution, maxResolution)
            .Without(x => x.AccessibleContentIds)
            .Create();

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(command));
        
        // assert
        Assert.Contains(errorMsg, exception.Message);
    }

    [Fact]
    public async Task AddSubcription_NonExistingContentInAccesibleContentGiven_ExceptionThrown()
    {
        // arrange
        var content = _fixture.Build<ContentBase>()
            .With(x => x.Id, 0)
            .OmitAutoProperties()
            .CreateMany(5);
        
        var command = _fixture.Build<CreateSubscriptionCommand>()
            .With(x => x.AccessibleContentIds, [1])
            .Create();

        _mockContentRepo.Setup(x => x.GetContentByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) => content.FirstOrDefault(x => x.Id == id));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(command));
        
        // assert
        Assert.Contains(SubscriptionErrorMessages.GivenIdOfNonExistingContent, exception.Message);
    }
    
    [Fact]
    public async Task DeleteSubscription_ExistingIdGiven_EntityDeleted()
    {
        // arrange
        const int idToDelete = 2;
        var subscriptions = new List<Subscription>()
        {
            _fixture.Build<Subscription>()
                .With(x => x.Id, 1)
                .Without(x => x.AccessibleContent)
                .Create(),
            _fixture.Build<Subscription>()
                .With(x => x.Id, 2)
                .Without(x => x.AccessibleContent)
                .Create()
        };
        
        _mockSubRepo
            .Setup(x => x.GetSubscriptionByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int i) => subscriptions.FirstOrDefault(x => x.Id == i));
        _mockSubRepo
            .Setup(x => x.Remove(It.IsAny<Subscription>()))
            .Callback((Subscription sub) => { subscriptions.Remove(sub); })
            .Returns((Subscription sub) => sub);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var subscription = await mediator.Send(new DeleteSubcriptionCommand(idToDelete));
        
        // assert
        _mockSubRepo.Verify(x => x.Remove(subscription), Times.Once);
        Assert.True(subscriptions.FirstOrDefault(x => x.Id == idToDelete) is null);
    }

    [Fact]
    public async Task DeleteSubscription_NonExistingIdGiven_ExceptionThrown()
    {
        // arrange
        const int idToDelete = 3;
        var subscriptions = new List<Subscription>()
        {
            _fixture.Build<Subscription>()
                .With(x => x.Id, 1)
                .Without(x => x.AccessibleContent)
                .Create(),
            _fixture.Build<Subscription>()
                .With(x => x.Id, 2)
                .Without(x => x.AccessibleContent)
                .Create()
        };

        
        _mockSubRepo
            .Setup(x => x.GetSubscriptionByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int i) => subscriptions.FirstOrDefault(x => x.Id == i));
        _mockSubRepo
            .Setup(x => x.Remove(It.IsAny<Subscription>()))
            .Callback((Subscription sub) => { subscriptions.Remove(sub); })
            .Returns((Subscription sub) => sub);

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(new DeleteSubcriptionCommand(idToDelete)));
        
        // assert
        _mockSubRepo.Verify(x => x.Remove(It.IsAny<Subscription>()), Times.Never);
        Assert.Contains(SubscriptionErrorMessages.SubscriptionNotFound, exception.Message);
    }

    [Fact]
    public async Task EditSubscription_CorrectDtoGiven_SubscriptionChanged()
    {
        // arrange
        var i = 1;
        var contents = GetContents(10);
        var contentIds = contents.Select(x => x.Id).ToList();
        
        var subscriptions = _fixture.Build<Subscription>()
            .With(x => x.Id, () => i++)
            .With(x => x.AccessibleContent, contents[..3])
            .CreateMany(5)
            .ToList();
        var command = _fixture.Build<EditSubscriptionCommand>()
            .With(x => x.SubscriptionId, 1)
            .With(x => x.AccessibleContentIdsToAdd, contentIds[3..5])
            .With(x => x.AccessibleContentIdsToRemove, contentIds[5..])
            .Create();

        _mockContentRepo.Setup(x => x.GetContentByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) => contents.FirstOrDefault(x => x.Id == id));
        _mockSubRepo.Setup(x => x.GetSubscriptionWithAccessibleContentAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => subscriptions.FirstOrDefault(x => x.Id == id));

        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var subscription = await mediator.Send(command);
        
        // assert
        if (command.NewName is not null)
            Assert.Equal(command.NewName, subscription.Name);
        
        if (command.NewDescription is not null)
            Assert.Equal(command.NewDescription, subscription.Description);
        
        if (command.NewMaxResolution is not null)
            Assert.Equal(command.NewMaxResolution, subscription.MaxResolution);
        
        foreach (var id in command.AccessibleContentIdsToAdd!)
        {
            Assert.True(subscription.AccessibleContent.SingleOrDefault(x => x.Id == id) is not null);
        }
        
        foreach (var id in command.AccessibleContentIdsToRemove!)
        {
            Assert.True(subscription.AccessibleContent.TrueForAll(x => x.Id != id));
        }
    }

    [Fact]
    public async Task EditSubscription_InvalidSubscriptionIdGiven_ExceptionThrown()
    {
        // arrange
        var i = 1;
        var subscriptions = _fixture.Build<Subscription>()
            .With(x => x.Id, () => i++)
            .Without(x => x.AccessibleContent)
            .CreateMany()
            .ToList();

        var command = _fixture.Build<EditSubscriptionCommand>()
            .With(x => x.SubscriptionId, 0)
            .Create();

        _mockSubRepo.Setup(x => x.GetSubscriptionWithAccessibleContentAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => subscriptions.FirstOrDefault(x => x.Id == id));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(command));
        
        // assert
        Assert.Contains(SubscriptionErrorMessages.SubscriptionNotFound, exception.Message);
    }

    [Theory]
    [InlineData("", "a", 480, SubscriptionErrorMessages.NotValidSubscriptionName)]
    [InlineData("NETFLIX(!)", "a", 480, SubscriptionErrorMessages.NotValidSubscriptionName)]
    [InlineData("a", "", 480, SubscriptionErrorMessages.NotValidSubscriptionDescription)]
    [InlineData("a", "a", 0, SubscriptionErrorMessages.NotValidSubscriptionMaxResolution)]
    [InlineData("a", "a", -1080, SubscriptionErrorMessages.NotValidSubscriptionMaxResolution)]

    public async Task EditSubscription_InvalidDtoGiven_ExceptionThrown(string name, string desc, int maxResolution, string errorMsg)
    {
        // arrange
        var command = _fixture.Build<EditSubscriptionCommand>()
            .With(x => x.NewName, name)
            .With(x => x.NewDescription, desc)
            .With(x => x.NewMaxResolution, maxResolution)
            .Without(x => x.AccessibleContentIdsToAdd)
            .Without(x => x.AccessibleContentIdsToRemove)
            .Create();

        _mockSubRepo.Setup(x => x.GetSubscriptionWithAccessibleContentAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Subscription() { Id = id });
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(command));
        
        // assert
        Assert.Contains(errorMsg, exception.Message);
    }
    
    [Fact]
    public async Task EditSubcription_NonExistingContentToAddIdGiven_ExceptionThrown()
    {
        // arrange
        var content = _fixture.Build<ContentBase>()
            .With(x => x.Id, 0)
            .OmitAutoProperties()
            .CreateMany(5);
        
        var command = _fixture.Build<EditSubscriptionCommand>()
            .With(x => x.AccessibleContentIdsToAdd, [1])
            .Without(x => x.AccessibleContentIdsToRemove)
            .Create();

        _mockSubRepo.Setup(x => x.GetSubscriptionWithAccessibleContentAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Subscription() { Id = id });
        _mockContentRepo.Setup(x => x.GetContentByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) => content.FirstOrDefault(x => x.Id == id));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(command));
        
        // assert
        Assert.Contains(SubscriptionErrorMessages.GivenIdOfNonExistingContent, exception.Message);
    }
    
    [Fact]
    public async Task EditSubcription_NonExistingContentToRemoveIdGiven_ExceptionThrown()
    {
        // arrange
        var content = _fixture.Build<ContentBase>()
            .With(x => x.Id, 0)
            .OmitAutoProperties()
            .CreateMany(5);
        
        var dto = _fixture.Build<EditSubscriptionCommand>()
            .With(x => x.AccessibleContentIdsToRemove, [2])
            .Without(x => x.AccessibleContentIdsToAdd)
            .Create();

        _mockSubRepo.Setup(x => x.GetSubscriptionWithAccessibleContentAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Subscription() { Id = id });
        _mockContentRepo.Setup(x => x.GetContentByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) => content.FirstOrDefault(x => x.Id == id));
        
        var mediator = _serviceProvider.GetService<IMediator>()!;
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentValidationException>(
            async () => await mediator.Send(dto));
        
        // assert
        Assert.Contains(SubscriptionErrorMessages.GivenIdOfNonExistingContent, exception.Message);
    }
    
    private List<ContentBase> GetContents(int count)
    {
        return _fixture.Build<ContentBase>()
            .With(x => x.Id)
            .OmitAutoProperties()
            .CreateMany(count)
            .ToList();
    }
}