using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Commands;
public class UpdateItemHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryStub;
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly UpdateItemHandler _handler;

    public UpdateItemHandlerTests()
    {
        _itemRepositoryStub = new Mock<IItemRepository>();
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new UpdateItemHandler(_itemRepositoryStub.Object, _itemTypeRepositoryStub.Object, _categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidItem_ReturnsUpdatedItem()
    {
        // Arrange
        var ancestorCategories = new List<Category>
        {
            Category.Reconstitute(1L, "ParentCategory")
        };
        var childCategory = Category.Reconstitute(2L, "ChildCategory", 1L, "1");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "");
        var existingItem = Item.Reconstitute(1L, new ItemParameters(
            "Old Item",
            "OI001",
            ancestorCategories[0],
            new Money(50m, "USD"),
            itemType,
            "Piece",
            true,
            "An old item"
        ));
        var command = new UpdateItemCommand(
            Id: existingItem.Id,
            Name: "Updated Item",
            Code: "UI001",
            CategoryId: childCategory.Id,
            ListPrice: new MoneyDto(150m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Box",
            IsActive: false,
            Description: "An updated item"
        );
        _itemRepositoryStub.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryRepositoryStub.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(childCategory);
        _categoryRepositoryStub.Setup(r => r.GetAncestorsAsync(command.CategoryId))
            .ReturnsAsync(ancestorCategories);
        _itemTypeRepositoryStub.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(itemType);
        _itemRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Item>()))
            .ReturnsAsync(true);
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
            Category = new
            {
                Id = 2L,
                Ancestors = new[]
                {
                new { Id = 1L, Name = "ParentCategory" }
            }
            },
            ItemType = new { Id = 1L, Name = "ItemType" }
        });
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(command.CategoryId), Times.Once);
        _categoryRepositoryStub.Verify(r => r.GetAncestorsAsync(command.CategoryId), Times.Once);
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(command.ItemTypeId), Times.Once);
        _itemRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateItemCommand(
            Id: 999L,
            Name: "Updated Item",
            Code: "UI001",
            CategoryId: 1L,
            ListPrice: new MoneyDto(150m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Box",
            IsActive: false,
            Description: "An updated item"
        );
        _itemRepositoryStub.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Item?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _itemRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var category = Category.Reconstitute(1L, "ChildCategory");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "");
        var existingItem = Item.Reconstitute(1L, new ItemParameters(
            "Old Item",
            "OI001",
            category,
            new Money(50m, "USD"),
            itemType,
            "Piece",
            true,
            "An old item"
        ));
        var command = new UpdateItemCommand(
            Id: existingItem.Id,
            Name: "Updated Item",
            Code: "UI001",
            CategoryId: 2L,
            ListPrice: new MoneyDto(150m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Box",
            IsActive: false,
            Description: "An updated item"
        );
        _itemRepositoryStub.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryRepositoryStub.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync((Category?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.CategoryId.ToString()));
        _itemRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ItemTypeNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var category = Category.Reconstitute(1L, "ChildCategory");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "");
        var existingItem = Item.Reconstitute(1L, new ItemParameters(
            "Old Item",
            "OI001",
            category,
            new Money(50m, "USD"),
            itemType,
            "Piece",
            true,
            "An old item"
        ));
        var command = new UpdateItemCommand(
            Id: existingItem.Id,
            Name: "Updated Item",
            Code: "UI001",
            CategoryId: 2L,
            ListPrice: new MoneyDto(150m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Box",
            IsActive: false,
            Description: "An updated item"
        );
        _itemRepositoryStub.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryRepositoryStub.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(category);
        _itemTypeRepositoryStub.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync((ItemType?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.ItemTypeId.ToString()));
        _itemRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var category = Category.Reconstitute(1L, "ChildCategory");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "");
        var existingItem = Item.Reconstitute(1L, new ItemParameters(
            "Old Item",
            "OI001",
            category,
            new Money(50m, "USD"),
            itemType,
            "Piece",
            true,
            "An old item"
        ));
        var command = new UpdateItemCommand(
            Id: existingItem.Id,
            Name: "Updated Item",
            Code: "UI001",
            CategoryId: 1L,
            ListPrice: new MoneyDto(150m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Box",
            IsActive: false,
            Description: "An updated item"
        );
        _itemRepositoryStub.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingItem);
        _categoryRepositoryStub.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(category);
        _itemTypeRepositoryStub.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(itemType);
        _itemRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Item>()))
            .ReturnsAsync(false);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
    }

}
