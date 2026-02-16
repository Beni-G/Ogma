using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;

public class GetCategoryByIdHandlerTests
{
    private readonly Mock<ICategoryReader> _categoryReaderStub;
    private readonly GetCategoryByIdHandler _handler;

    public GetCategoryByIdHandlerTests()
    {
        _categoryReaderStub = new Mock<ICategoryReader>();
        _handler = new GetCategoryByIdHandler(_categoryReaderStub.Object);
    }

    [Fact]
    public async Task Handle_CategoryExists_ReturnsCategoryWithDescendantsDto()
    {
        // Arrange
        var parentCategoryId = CatalogTestData.NextId();
        var path = $"{parentCategoryId}";
        var subCategory = new CategoryDto(
            Id: CatalogTestData.NextId(),
            Name: CatalogTestData.CreateName(),
            ParentCategoryId: parentCategoryId,
            Path: $"{path}/{CatalogTestData.NextId()}"
        );
        var category = new CategoryDto(
            Id: parentCategoryId,
            Name: CatalogTestData.CreateName(),
            ParentCategoryId: null,
            Path: string.Empty,
            Ancestors: null,
            SubCategories: new List<CategoryDto>() { subCategory }
        );

        _categoryReaderStub.Setup(repo => repo.GetByIdAsync(category.Id))
            .ReturnsAsync(category);
        // Act
        var result = await _handler.Handle(new GetCategoryByIdQuery(category.Id), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.SubCategories.Should().HaveCount(1);
        var subCategoryDto = result.SubCategories.First();
        subCategoryDto.Id.Should().Be(subCategory.Id);
        subCategoryDto.Name.Should().Be(subCategory.Name);
        _categoryReaderStub.Verify(r => r.GetByIdAsync(category.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        long id = CatalogTestData.NextId();
        _categoryReaderStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((CategoryDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _categoryReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
