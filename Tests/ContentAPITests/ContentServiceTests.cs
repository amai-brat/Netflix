using AutoFixture;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Services;
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
            var availableContent = BuildDefaultContentBaseList();
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
            var availableContent = BuildDefaultContentBaseList();
            var id = -1;

            //Act
            _mockContent.Setup(repository => repository.GetContentByFilterAsync(It.IsAny<Expression<Func<ContentBase, bool>>>()))
                .ReturnsAsync((Expression<Func<ContentBase, bool>> filter) => availableContent.SingleOrDefault(filter.Compile()));

            var service = new ContentService(_mockContent.Object);
            var result = await service.GetContentByIdAsync(id);

            //Assert
            Assert.Null(result);
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
    }
}
