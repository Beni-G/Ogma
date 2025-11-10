using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Commands;
public class CreateItemHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CreateItemHandler _handler;

    public CreateItemHandlerTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _itemTypeRepositoryMock = new Mock<IItemTypeRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new CreateItemHandler(_itemRepositoryMock.Object, _itemTypeRepositoryMock.Object, _categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidItem_ReturnsCreatedItem()
    {
        // Arrange
        var ancestorCategories = new List<Category>
        {
            Category.Reconstitute(1L, "ParentCategory")
        };
        var childCategory = Category.Reconstitute(2L, "ChildCategory", 1L, "1");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "");
        var command = new CreateItemCommand(
            Name: "Test Item",
            Code: "TI001",
            CategoryId: childCategory.Id,
            ListPrice: new MoneyDto(100m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A test item"
        );
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(childCategory);
        _categoryRepositoryMock.Setup(r => r.GetAncestorsAsync(command.CategoryId))
            .ReturnsAsync(ancestorCategories);
        _itemTypeRepositoryMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(itemType);
        _itemRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Item>()))
            .ReturnsAsync((Item item) =>
            {
                return Item.Reconstitute(1L, new ItemParameters(
                    command.Name,
                    command.Code,
                    childCategory,
                    new Money(command.ListPrice.Amount, command.ListPrice.Currency),
                    itemType,
                    command.UnitOfMeasurement,
                    command.IsActive,
                    command.Description));
            });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            Id = 1L,
            Name = command.Name,
            Code = command.Code,
            Description = command.Description,
            IsActive = true,
            UnitOfMeasurement = command.UnitOfMeasurement,
            ListPrice = new { Amount = 100m, Currency = "USD" },
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
        _categoryRepositoryMock.Verify(r => r.GetByIdAsync(command.CategoryId), Times.Once);
        _categoryRepositoryMock.Verify(r => r.GetAncestorsAsync(command.CategoryId), Times.Once);
        _itemTypeRepositoryMock.Verify(r => r.GetByIdAsync(command.ItemTypeId), Times.Once);
        _itemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CreateItemCommand(
            Name: "Test Item",
            Code: "TI001",
            CategoryId: 999L, // Non-existing category
            ListPrice: new MoneyDto(100m, "USD"),
            ItemTypeId: 1L,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A test item"
        );
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync((Category?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.CategoryId.ToString()));
        _itemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ItemTypeNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var category = Category.Reconstitute(2L, "ChildCategory", 1L, "1");
        var command = new CreateItemCommand(
            Name: "Test Item",
            Code: "TI001",
            CategoryId: category.Id,
            ListPrice: new MoneyDto(100m, "USD"),
            ItemTypeId: 999L, // Non-existing item type
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A test item"
        );
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(category);
        _itemTypeRepositoryMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync((ItemType?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.ItemTypeId.ToString()));
        _itemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
    }
}
