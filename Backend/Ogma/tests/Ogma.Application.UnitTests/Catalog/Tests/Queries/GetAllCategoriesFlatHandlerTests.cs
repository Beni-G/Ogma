using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;
public class GetAllCategoriesFlatHandlerTests
{
    private readonly Mock<ICategoryReader> _categoryReaderStub;
    private readonly GetAllCategoriesFlatHandler _handler;

    public GetAllCategoriesFlatHandlerTests()
    {
        _categoryReaderStub = new Mock<ICategoryReader>();
        _handler = new GetAllCategoriesFlatHandler(_categoryReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllCategoriesFlatList()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            CatalogTestData.CreateCategoryDto(),
            CatalogTestData.CreateCategoryDto(),
            CatalogTestData.CreateCategoryDto()
        };
        _categoryReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);
        // Act
        var result = await _handler.Handle(new GetAllCategoriesFlatQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(categories.Count);
        _categoryReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoCategories_ReturnsEmptyList()
    {
        // Arrange
        var categories = new List<CategoryDto>();
        _categoryReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);
        // Act
        var result = await _handler.Handle(new GetAllCategoriesFlatQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        _categoryReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

}
