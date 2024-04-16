using Application.Exceptions;
using Application.Repositories;
using Application.Services.Implementations;
using Domain.Entities;
using AutoFixture;
using Moq;

namespace Tests.ContentAPITests
{
	public class CommentServiceTests
	{
		private readonly Fixture _fixture = new();
		private readonly Mock<ICommentRepository> _mockComment = new();
		private readonly Mock<IReviewRepository> _mockReview = new();
		private readonly Mock<IUserRepository> _mockUser = new();

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

			var service = new CommentService(_mockComment.Object, _mockReview.Object, _mockUser.Object);

			//Act
			var deletedComment = await service.DeleteCommentByIdAsync(commentId);

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

			var service = new CommentService(_mockComment.Object, _mockReview.Object, _mockUser.Object);

			//Act
			var ex = await Assert.ThrowsAsync<CommentServiceArgumentException>(async() =>
				await service.DeleteCommentByIdAsync(notExistingId));

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
}
