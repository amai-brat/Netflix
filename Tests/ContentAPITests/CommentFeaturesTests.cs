using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Comments.Commands.DeleteComment;
using Application.Repositories;
using AutoFixture;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.ContentAPITests;

public class CommentFeaturesTests
{
    private readonly Fixture _fixture = new();
	private readonly Mock<ICommentRepository> _mockComment = new();
	private readonly Mock<IReviewRepository> _mockReview = new();
	private readonly Mock<IUserRepository> _mockUser = new();
	private readonly IServiceProvider _serviceProvider;

	public CommentFeaturesTests()
	{
		_serviceProvider = new TestServiceProviderBuilder()
			.With(services =>
			{
				services.AddScoped<ICommentRepository>(_ => _mockComment.Object);
				services.AddScoped<IReviewRepository>(_ => _mockReview.Object);
				services.AddScoped<IUserRepository>(_ => _mockUser.Object);
			})
			.Build();
	}

	[Fact]
	public async Task DeleteComment_CorrectIdGiven_CommentReturned()
	{
		//Arrange
		var comments = BuildDefaultCommentsList();
		var comment = comments[Random.Shared.Next(0, comments.Count)];
		var commentId = comment.Id;

		_mockComment.Setup(repository => repository.GetCommentByIdAsync(It.IsAny<long>()))
			.ReturnsAsync((long id) => comments.SingleOrDefault(com => com.Id == id));
		_mockComment.Setup(repository => repository.Remove(It.IsAny<Comment>()))
			.Returns((Comment com) =>
			{
				comments.Remove(com);
				return com;
			});

		var mediator = _serviceProvider.GetService<IMediator>()!;

		//Act
		var deletedComment = await mediator.Send(new DeleteCommentCommand(commentId));

		//Assert
		Assert.Equal(comment, deletedComment);
		Assert.DoesNotContain(comment, comments);
	}


	[Fact]
	public async Task DeleteComment_InvalidIdGiven_ExceptionThrown()
	{
		//Arrange
		var comments = BuildDefaultCommentsList();
		var notExistingId = -1000L;

		_mockComment.Setup(repository => repository.GetCommentByIdAsync(It.IsAny<long>()))
			.ReturnsAsync((long id) => comments.SingleOrDefault(comment => comment.Id == id));
		_mockComment.Setup(repository => repository.Remove(It.IsAny<Comment>()))
			.Returns((Comment comment) =>
			{
				comments.Remove(comment);
				return comment;
			});
		
		var mediator = _serviceProvider.GetService<IMediator>()!;
		
		//Act
		var ex = await Assert.ThrowsAsync<ArgumentValidationException>(async() =>
			await mediator.Send(new DeleteCommentCommand(notExistingId)));

		//Assert
		Assert.Contains(ErrorMessages.NotFoundComment, ex.Message);
	}

	private List<Comment> BuildDefaultCommentsList()
	{
		var i = 1;
		return _fixture.Build<Comment>()
			.With(x => x.Id, () => i++)
			.OmitAutoProperties()
			.CreateMany(10)
			.ToList();
	}
}