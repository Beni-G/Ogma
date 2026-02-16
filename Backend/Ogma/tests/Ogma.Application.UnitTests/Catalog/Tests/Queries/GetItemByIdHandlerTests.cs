using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;
using Ogma.Domain.Catalog.Entities;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;

public class GetItemByIdHandlerTests
{
    private readonly Mock<IItemReader> _itemReaderStub;
    private readonly Mock<ICategoryReader> _categoryReaderStub;
    private readonly Mock<IItemTypeReader> _itemTypeReaderStub;
    private readonly GetItemByIdHandler _handler;

    public GetItemByIdHandlerTests()
    {
        _itemReaderStub = new Mock<IItemReader>();
        _categoryReaderStub = new Mock<ICategoryReader>();
        _itemTypeReaderStub = new Mock<IItemTypeReader>();
        _handler = new GetItemByIdHandler(_itemReaderStub.Object, _categoryReaderStub.Object, _itemTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsItem()
    {
        // Arrange
        var ancestorCategoryEntity = Category.Reconstitute(CatalogTestData.NextId(), CatalogTestData.CreateName());
        var categoryEntity = Category.Reconstitute(CatalogTestData.NextId(), CatalogTestData.CreateName(), ancestorCategoryEntity.Id, $"{ancestorCategoryEntity.Id}");
        var ancestorCategories = new List<CategoryDto> { ancestorCategoryEntity.ToDto() };
        var category = categoryEntity.ToDto(ancestorCategories);
        var itemType = CatalogTestData.CreateItemTypeDto();
        var itemNoCategory = CatalogTestData.CreateItemDto(category.Id, itemType.Id);
        var item = itemNoCategory with { Category = category, ItemType = itemType };
        _itemReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(item);
        _categoryReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(category);
        _categoryReaderStub.Setup(r => r.GetAncestorsAsync(It.IsAny<long>()))
            .ReturnsAsync(ancestorCategories);
        _itemTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(itemType);
        // Act
        var result = await _handler.Handle(new GetItemByIdQuery(item.Id), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Code.Should().Be(item.Code);
        result.Category.Should().NotBeNull();
        result.Category!.Id.Should().Be(item.Category.Id);
        result.Category.Should().BeEquivalentTo(item.Category, options => options.Excluding(c => c.SubCategories));
        result.ItemType.Should().NotBeNull();
        result.ItemType!.Id.Should().Be(item.ItemType.Id);
        result.ItemType.Should().BeEquivalentTo(item.ItemType);
        _categoryReaderStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _categoryReaderStub.Verify(r => r.GetAncestorsAsync(It.IsAny<long>()), Times.Once);
        _itemReaderStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingItem_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = CatalogTestData.NextId();
        var query = new GetItemByIdQuery(id);
        _itemReaderStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ItemDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _itemReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ReturnsKeyNotFoundException()
    {
        // Arrange
        var item = CatalogTestData.CreateItemDto(CatalogTestData.NextId(), CatalogTestData.NextId());
        var query = new GetItemByIdQuery(item.Id);
        _itemReaderStub.Setup(repo => repo.GetByIdAsync(item.Id)).ReturnsAsync(item);
        _categoryReaderStub.Setup(repo => repo.GetByIdAsync(item.CategoryId)).ReturnsAsync((CategoryDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(item.CategoryId.ToString()));
        _itemReaderStub.Verify(r => r.GetByIdAsync(item.Id), Times.Once);
        _categoryReaderStub.Verify(r => r.GetByIdAsync(item.CategoryId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingItemType_ReturnsKeyNotFoundException()
    {
        // Arrange
        var categoryEntity = Category.Reconstitute(CatalogTestData.NextId(), CatalogTestData.CreateName());
        var category = categoryEntity.ToDto();
        var item = CatalogTestData.CreateItemDto(category.Id, CatalogTestData.NextId());
        var query = new GetItemByIdQuery(item.Id);
        _itemReaderStub.Setup(repo => repo.GetByIdAsync(item.Id)).ReturnsAsync(item);
        _categoryReaderStub.Setup(repo => repo.GetByIdAsync(item.CategoryId)).ReturnsAsync(category);
        _itemTypeReaderStub.Setup(repo => repo.GetByIdAsync(item.ItemTypeId)).ReturnsAsync((ItemTypeDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(item.ItemTypeId.ToString()));
        _itemReaderStub.Verify(r => r.GetByIdAsync(item.Id), Times.Once);
        _categoryReaderStub.Verify(r => r.GetByIdAsync(item.CategoryId), Times.Once);
        _itemTypeReaderStub.Verify(r => r.GetByIdAsync(item.ItemTypeId), Times.Once);
    }
}
