using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Queries;
public class GetAllCategoriesFlatHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly GetAllCategoriesFlatHandler _handler;

    public GetAllCategoriesFlatHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new GetAllCategoriesFlatHandler(_categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllCategoriesFlatList()
    {
        // Arrange
        var categories = new List<Category>
        {
            Category.Reconstitute(1L, "Category1", null, ""),
            Category.Reconstitute(2L, "Category2", null, ""),
            Category.Reconstitute(3L, "Category2.1", 2L, "2"),
            Category.Reconstitute(4L, "Category2.1.1", 3L, "2/3")
        };
        _categoryRepositoryStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);
        // Act
        var result = await _handler.Handle(new GetAllCategoriesFlatQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(4);
        result[0].Id.Should().Be(1L);
        result[0].Name.Should().Be("Category1");
        result[0].ParentCategoryId.Should().BeNull();
        result[0].Path.Should().Be("");
        result[1].Id.Should().Be(2L);
        result[1].Name.Should().Be("Category2");
        result[1].ParentCategoryId.Should().BeNull();
        result[1].Path.Should().Be("");
        result[2].Id.Should().Be(3L);
        result[2].Name.Should().Be("Category2.1");
        result[2].ParentCategoryId.Should().Be(2L);
        result[2].Path.Should().Be("2");
        result[3].Id.Should().Be(4L);
        result[3].Name.Should().Be("Category2.1.1");
        result[3].ParentCategoryId.Should().Be(3L);
        result[3].Path.Should().Be("2/3");
        _categoryRepositoryStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoCategories_ReturnsEmptyList()
    {
        // Arrange
        var categories = new List<Category>();
        _categoryRepositoryStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);
        // Act
        var result = await _handler.Handle(new GetAllCategoriesFlatQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        _categoryRepositoryStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

}
