using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;

public class GetAllCategoriesTreeHandlerTests
{
    private readonly Mock<ICategoryReader> _categoryReaderMock;
    private readonly GetAllCategoriesTreeHandler _handler;

    public GetAllCategoriesTreeHandlerTests()
    {
        _categoryReaderMock = new Mock<ICategoryReader>();
        _handler = new GetAllCategoriesTreeHandler(_categoryReaderMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMappedCategoriesTree()
    {
        // Arrange
        var child = CatalogTestData.CreateCategoryDto();
        var root = CatalogTestData.CreateCategoryDto() with
        {
            SubCategories = new List<CategoryDto> { child }
        };

        var categoryTree = new List<CategoryDto> { root };

        _categoryReaderMock
            .Setup(r => r.GetAllCategoriesTreeAsync())
            .ReturnsAsync(categoryTree);

        var handler = new GetAllCategoriesTreeHandler(_categoryReaderMock.Object);
        // Act
        var result = await handler.Handle(new GetAllCategoriesTreeQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(categoryTree.Count);
        result.Should().BeEquivalentTo(categoryTree);
        result[0].SubCategories.Should().ContainSingle();
        _categoryReaderMock.Verify(r => r.GetAllCategoriesTreeAsync(), Times.Once);
    }


    [Fact]
    public async Task Handle_NoCategories_ReturnsEmptyList()
    {
        // Arrange
        var categories = new List<CategoryDto>();
        _categoryReaderMock.Setup(repo => repo.GetAllCategoriesTreeAsync())
            .ReturnsAsync(categories);
        // Act
        var result = await _handler.Handle(new GetAllCategoriesTreeQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        _categoryReaderMock.Verify(r => r.GetAllCategoriesTreeAsync(), Times.Once);
    }
}
