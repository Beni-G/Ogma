using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Queries;
public class GetAllCategoriesTreeHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly GetAllCategoriesTreeHandler _handler;

    public GetAllCategoriesTreeHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new GetAllCategoriesTreeHandler(_categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMappedCategoriesTree()
    {
        // Arrange
        var domainTree = new List<Category>
        {
            Category.Reconstitute(1L, "Parent", null, ""),
        };
        domainTree[0].AddSubCategory(Category.Reconstitute(2L, "Child", 1L, "1"));

        _categoryRepositoryStub
            .Setup(r => r.GetAllCategoriesTreeAsync())
            .ReturnsAsync(domainTree);

        var handler = new GetAllCategoriesTreeHandler(_categoryRepositoryStub.Object);
        // Act
        var result = await handler.Handle(new GetAllCategoriesTreeQuery(), CancellationToken.None);
        // Assert
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1L);
        result[0].SubCategories.Should().ContainSingle()
            .Which.Id.Should().Be(2L);

        _categoryRepositoryStub.Verify(r => r.GetAllCategoriesTreeAsync(), Times.Once);
    }


    [Fact]
    public async Task Handle_NoCategories_ReturnsEmptyList()
    {
        // Arrange
        var categories = new List<Category>();
        _categoryRepositoryStub.Setup(repo => repo.GetAllCategoriesTreeAsync())
            .ReturnsAsync(categories);
        // Act
        var result = await _handler.Handle(new GetAllCategoriesTreeQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        _categoryRepositoryStub.Verify(r => r.GetAllCategoriesTreeAsync(), Times.Once);
    }
}
