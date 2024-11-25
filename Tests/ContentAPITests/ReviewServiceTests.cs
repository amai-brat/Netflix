using AutoFixture;
using Domain.Entities;
using Moq;
using System.Linq.Expressions;
using Application.Cache;
using Application.Dto;
using Application.Exceptions.ErrorMessages;
using Application.Exceptions.Particular;
using Application.Repositories;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using AutoMapper;
using Infrastructure.Profiles;
using Tests.Customizations;
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable InconsistentNaming

namespace Tests.ContentAPITests
{
    [Obsolete("CQRS")]
    public class ReviewServiceTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IContentRepository> _mockContent = new();
        private readonly Mock<IUserRepository> _mockUser = new();
        private readonly Mock<IReviewRepository> _mockReview = new();
        private readonly Mock<IMinioCache> _mockMinioCache = new();
        private readonly Mock<IUserService> _mockUserService = new();
        private readonly IMapper _mapper;
        
        public ReviewServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ReviewProfile());
                mc.AddProfile(new FavouriteProfile());
                mc.AddProfile(new ContentProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());
        }

        [Fact]
        public async Task AssignReviewWithCorrectDataShouldWorkCorrect()
        {
            //Arrange
            var availableContent = BuildDefaultContentBaseList();
            var users = BuildDefaultUserList();
            var contentId = availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
            var userId = users[Random.Shared.Next(0, users.Count)].Id;
            var userReviews = new List<Review>();
            var review = new ReviewAssignDto()
            {
                ContentId = contentId,
                IsPositive = _fixture.Create<bool>(),
                Score = _fixture.Create<int>() % 11,
                Text = _fixture.Create<string>()
            };

            //Act
            _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
            _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
            _mockReview.Setup(repository => repository.AssignReviewAsync(It.IsAny<Review>()))
                .Callback((Review rev) => { userReviews.Add(rev); });

            var service = GetService();
            await service.AssignReviewAsync(review, userId);

            //Assert
            Assert.Equal(userId, userReviews[0].UserId);
            Assert.Equal(review.ContentId, userReviews[0].ContentId);
            Assert.Equal(review.Score, userReviews[0].Score);
            Assert.Equal(review.IsPositive, userReviews[0].IsPositive);
            Assert.Equal(review.Text, userReviews[0].Text);
        }

        [Fact]
        public async Task AssignReviewWithScoreWithCorrectDataShouldWorkCorrect()
        {
            //Arrange
            var availableContent = BuildDefaultContentBaseList();
            var users = BuildDefaultUserList();
            var contentId = availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
            var userId = users[Random.Shared.Next(0, users.Count)].Id;
            var userReviews = new List<Review>();
            var review = new ReviewAssignDto()
            {
                ContentId = contentId,
                IsPositive = _fixture.Create<bool>(),
                Score = _fixture.Create<int>() % 11,
                Text = _fixture.Create<string>()
            };

            //Act
            _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
            _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
            _mockReview.Setup(repository => repository.AssignReviewAsync(It.IsAny<Review>()))
                .Callback((Review rev) => { userReviews.Add(rev); });

            var service = GetService();
            await service.AssignReviewWithRatingAsync(review, userId);

            //Assert
            Assert.Equal(userId, userReviews[0].UserId);
            Assert.Equal(review.ContentId, userReviews[0].ContentId);
            Assert.Equal(review.Score, userReviews[0].Score);
            Assert.Equal(review.IsPositive, userReviews[0].IsPositive);
            Assert.Equal(review.Text, userReviews[0].Text);
        }

        [Theory]
        [InlineData(-1, 0, "aa", 1, ErrorMessages.NotFoundUser)]
        [InlineData(0, -1, "aa", 1, ErrorMessages.NotFoundContent)]
        [InlineData(0, 0, "", 1, ErrorMessages.ReviewMustHaveText)]
        [InlineData(0, 0, null, 1, ErrorMessages.ReviewMustHaveText)]
        [InlineData(0, 0, "aa", -1, ErrorMessages.ScoreMustBeValid)]
        [InlineData(0, 0, "aa", 11, ErrorMessages.ScoreMustBeValid)]
        public async Task AssignReviewWithScoreWithInCorrectDataShouldThrowException(long userId, long contentId, string text, int score, string errorMsg)
        {
            //Arrange
            var availableContent = BuildDefaultContentBaseList();
            var users = BuildDefaultUserList();
            var _contentId = contentId == -1 ? contentId :  availableContent[Random.Shared.Next(0, availableContent.Count)].Id;
            var _userId = userId == -1 ? userId : users[Random.Shared.Next(0, users.Count)].Id;
            var userReviews = new List<Review>();
            var review = new ReviewAssignDto()
            {
                ContentId = _contentId,
                IsPositive = _fixture.Create<bool>(),
                Score = score,
                Text = text
            };

            //Act
            _mockUser.Setup(repository => repository.GetUserByFilterAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((Expression<Func<User, bool>> filter) => users.SingleOrDefault(filter.Compile()));
            _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));
            _mockReview.Setup(repository => repository.AssignReviewAsync(It.IsAny<Review>()))
                .Callback((Review rev) => { userReviews.Add(rev); });

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ReviewServiceArgumentException>(async () => { await service.AssignReviewWithRatingAsync(review, _userId); });

            //Assert
            Assert.Contains(errorMsg, ex.Message);
        }

        [Fact]
        public async Task GetReviewsByContentIdWithCorrectDataShouldWorkCorrect()
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var contentId = reviews[Random.Shared.Next(0, reviews.Count)].ContentId;

            //Act
            _mockReview.Setup(repository => repository.GetReviewsByFilterAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync((Expression<Func<Review, bool>> filter) =>  reviews.Where(filter.Compile()).ToList());

            var service = GetService();
            var result = await service.GetReviewsByContentIdAsync(contentId);

            //Assert
            Assert.True(result.All(r => r.ContentId == contentId));
        }


        [Fact]
        public async Task GetReviewsByContentIdWithSortWithCorrectDataShouldWorkCorrect()
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var contentId = reviews[Random.Shared.Next(0, reviews.Count)].ContentId;
            var sortType = "score";

            //Act
            _mockReview.Setup(repository => repository.GetReviewsByFilterAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync((Expression<Func<Review, bool>> filter) => reviews.Where(filter.Compile()).ToList());

            var service = GetService();
            var result = await service.GetReviewsByContentIdAsync(contentId, sortType);

            //Assert
            Assert.True(reviews.Where(r => r.ContentId == contentId).OrderBy(r => r.Score).SequenceEqual(result));
        }

        [Fact]
        public async Task GetReviewsByContentIdWithLimitWithCorrectDataShouldWorkCorrect()
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var contentId = reviews[Random.Shared.Next(0, reviews.Count)].ContentId;
            var sortType = "score";
            var offset = 0;
            var limit = 3;

            //Act
            _mockReview.Setup(repository => repository.GetReviewsByFilterAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync((Expression<Func<Review, bool>> filter) => reviews.Where(filter.Compile()).ToList());

            var service = GetService();
            var result = await service.GetReviewsByContentIdAsync(contentId, sortType, offset, limit);

            //Assert
            var toCheckReviews = reviews.Where(r => r.ContentId == contentId).OrderBy(r => r.Score).Take(limit).ToList();
            Assert.True(_mapper.Map<List<ReviewDto>>(toCheckReviews).SequenceEqual(result, new ReviewDtoEqualityComparer()));
            
        }

        [Fact]
        public async Task GetReviewsByContentIdWithInCorrectDataShouldReturnEmptyList()
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var contentId = -1;

            //Act
            _mockReview.Setup(repository => repository.GetReviewsByFilterAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync((Expression<Func<Review, bool>> filter) => reviews.Where(filter.Compile()).ToList());

            var service = GetService();
            var result = await service.GetReviewsByContentIdAsync(contentId);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReviewsByContentIdWithSortWithInCorrectDataShouldThrowException()
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var contentId = reviews[Random.Shared.Next(0, reviews.Count)].ContentId;
            var sortType = "asdfadsf";

            //Act
            _mockReview.Setup(repository => repository.GetReviewsByFilterAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync((Expression<Func<Review, bool>> filter) => reviews.Where(filter.Compile()).ToList());

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ReviewServiceArgumentException>(async () => { await service.GetReviewsByContentIdAsync(contentId, sortType); });

            //Assert
            Assert.Contains(ErrorMessages.IncorrectSortType, ex.Message);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        public async Task GetReviewsByContentIdWithLimitWithInCorrectDataShouldThrowException(int offset, int limit)
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var contentId = reviews[Random.Shared.Next(0, reviews.Count)].ContentId;
            var sortType = "score";

            //Act
            _mockReview.Setup(repository => repository.GetReviewsByFilterAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync((Expression<Func<Review, bool>> filter) => reviews.Where(filter.Compile()).ToList());

            var service = GetService();
            var ex = await Assert.ThrowsAsync<ReviewServiceArgumentException>(async () => { await service.GetReviewsByContentIdAsync(contentId, sortType, offset, limit); });

            //Assert
            Assert.Contains(ErrorMessages.ArgumentsMustBePositive, ex.Message);
        }

        [Fact]
        public async Task DeleteReview_CorrectIdGiven_ReviewReturned()
        {
            //Arrange
            var reviews = BuildDefaultReviewList();
            var review = reviews[Random.Shared.Next(0, reviews.Count)];
            var reviewId = review.Id;

            _mockReview.Setup(repository => repository.GetReviewByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((long id) => reviews.SingleOrDefault(r => r.Id == id));
            _mockReview.Setup(repository => repository.DeleteReview(It.IsAny<Review>()))
                .Returns((Review r) =>
                {
                    reviews.Remove(r);
                    return r;
                });

            var service = GetService();
            
			//Act
            var deletedReview = await service.DeleteReviewByIdAsync(reviewId);

            //Assert
            Assert.Equal(review, deletedReview);
            Assert.DoesNotContain(review, reviews);
		}

		[Fact]
		public async Task DeleteReview_InvalidIdGiven_ExceptionThrown()
		{
			//Arrange
			var reviews = BuildDefaultReviewList();
            var notExistingId = -1000L;

			_mockReview.Setup(repository => repository.GetReviewByIdAsync(It.IsAny<long>()))
				.ReturnsAsync((long id) => reviews.SingleOrDefault(review => review.Id == id));
			_mockReview.Setup(repository => repository.DeleteReview(It.IsAny<Review>()))
				.Returns((Review review) =>
				{
					reviews.Remove(review);
					return review;
				});

            var service = GetService();

			//Act
			var ex = await Assert.ThrowsAsync<ReviewServiceArgumentException>(async() => 
                await service.DeleteReviewByIdAsync(notExistingId));

            //Assert
            Assert.Contains(ErrorMessages.NotFoundReview, ex.Message);
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

        private List<Review> BuildDefaultReviewList() =>
            _fixture.Build<Review>()
            .Without(u => u.Content)
            .Without(u => u.Comments)
            .Without(u => u.RatedByUsers)
            .Without(u => u.WrittenAt)
            .Do(u => { u.Id = Math.Abs(u.Id); })
            .CreateMany(20)
            .ToList();

        private ReviewService GetService() => new ReviewService(
                _mockReview.Object, 
                _mockContent.Object, 
                _mockUser.Object, 
                _mockMinioCache.Object,
                _mockUserService.Object,
                _mapper);
        
        private class ReviewDtoEqualityComparer : IEqualityComparer<ReviewDto>
        {
            public bool Equals(ReviewDto? x, ReviewDto? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id && x.Score == y.Score && x.WrittenAt.Equals(y.WrittenAt) && x.LikesScore == y.LikesScore && x.IsPositive == y.IsPositive && x.Text == y.Text;
            }

            public int GetHashCode(ReviewDto obj)
            {
                return HashCode.Combine(obj.Id, obj.User, obj.Score, obj.WrittenAt, obj.LikesScore, obj.IsPositive, obj.Text, obj.Comments);
            }
        }
    }
}
