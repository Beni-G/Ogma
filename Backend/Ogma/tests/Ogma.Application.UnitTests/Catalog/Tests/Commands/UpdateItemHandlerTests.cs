using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Application.UnitTests.Catalog.Helpers;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Catalog.Tests.Commands;

public class UpdateItemHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly Mock<IItemReader> _itemReaderMock;
    private readonly Mock<IItemTypeReader> _itemTypeReaderMock;
    private readonly Mock<ICategoryReader> _categoryReaderMock;
    private readonly UpdateItemHandler _handler;

    public UpdateItemHandlerTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _itemReaderMock = new Mock<IItemReader>();
        _itemTypeReaderMock = new Mock<IItemTypeReader>();
        _categoryReaderMock = new Mock<ICategoryReader>();
        _handler = new UpdateItemHandler(_itemRepositoryMock.Object, _itemReaderMock.Object, _itemTypeReaderMock.Object, _categoryReaderMock.Object);
    }

    [Fact]
    public async Task Handle_ValidItem_ReturnsUpdatedItem()
    {
        // Arrange
        var existingItem = Item.Reconstitute(CatalogTestData.NextId(), CatalogTestData.CreateItemParameters());
        var ancestorCategories = CatalogTestData.CreateCategoryDtoAncestors();
        var newCategory = CatalogTestData.CreateCategoryDto(ancestors: ancestorCategories);
        var itemType = CatalogTestData.CreateItemTypeDto();
        var command = CatalogTestData.GenerateUpdateItemCommand(existingItem.Id, newCategory.Id, itemType.Id);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(newCategory);
        _categoryReaderMock.Setup(r => r.GetAncestorsAsync(command.CategoryId))
            .ReturnsAsync(ancestorCategories);
        _itemTypeReaderMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(itemType);
        _itemRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Item>()))
            .ReturnsAsync(true);
        _itemReaderMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(new ItemDto(
                Id: command.Id,
                Name: command.Name,
                Code: command.Code,
                Description: command.Description,
                CategoryId: command.CategoryId,
                Category: newCategory,
                ListPrice: new MoneyDto(command.ListPrice.Amount, command.ListPrice.Currency),
                ItemTypeId: command.ItemTypeId,
                ItemType: itemType,
                UnitOfMeasurement: command.UnitOfMeasurement,
                IsActive: command.IsActive
            ));
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            Id = command.Id,
            Name = command.Name,
            Code = command.Code,
            Description = command.Description,
            IsActive = command.IsActive,
            UnitOfMeasurement = command.UnitOfMeasurement,
            ListPrice = new { command.ListPrice.Amount, command.ListPrice.Currency },
            CategoryId = newCategory.Id,
            Category = newCategory,
            ItemTypeId = itemType.Id,
            ItemType = itemType
        });
        result.Category.Should().NotBeNull();
        result.Category?.Ancestors.Should().HaveCount(ancestorCategories.Count);
        result.ItemType.Should().NotBeNull();
        _categoryReaderMock.Verify(r => r.GetByIdAsync(command.CategoryId), Times.Once);
        _categoryReaderMock.Verify(r => r.GetAncestorsAsync(command.CategoryId), Times.Once);
        _itemTypeReaderMock.Verify(r => r.GetByIdAsync(command.ItemTypeId), Times.Once);
        _itemRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = CatalogTestData.GenerateUpdateItemCommand(CatalogTestData.NextId());
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Item?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _itemRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var existingItem = CatalogTestData.CreateItem();
        var command = CatalogTestData.GenerateUpdateItemCommand(existingItem.Id);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync((CategoryDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.CategoryId.ToString()));
        _itemRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ItemTypeNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var category = CatalogTestData.CreateCategoryDto();
        var existingItem = CatalogTestData.CreateItem();
        var command = CatalogTestData.GenerateUpdateItemCommand(existingItem.Id);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(category);
        _itemTypeReaderMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync((ItemTypeDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.ItemTypeId.ToString()));
        _itemRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var newCategory = CatalogTestData.CreateCategoryDto();
        var newItemType = CatalogTestData.CreateItemTypeDto();
        var existingItem = CatalogTestData.CreateItem();
        var command = CatalogTestData.GenerateUpdateItemCommand(existingItem.Id, newCategory.Id, newItemType.Id);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(newCategory);
        _itemTypeReaderMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(newItemType);
        _itemRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Item>()))
            .ReturnsAsync(false);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
    }

    [Fact]
    public async Task Handle_UpdatedItemNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var newCategory = CatalogTestData.CreateCategoryDto();
        var newItemType = CatalogTestData.CreateItemTypeDto();
        var existingItem = CatalogTestData.CreateItem();
        var command = CatalogTestData.GenerateUpdateItemCommand(existingItem.Id, newCategory.Id, newItemType.Id);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(newCategory);
        _itemTypeReaderMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(newItemType);
        _itemRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Item>()))
            .ReturnsAsync(true);
        _itemReaderMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((ItemDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
    }

}
