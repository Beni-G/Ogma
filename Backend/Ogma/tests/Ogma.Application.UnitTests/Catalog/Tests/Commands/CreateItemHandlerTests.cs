using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.UnitTests.Catalog.Helpers;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Catalog.Tests.Commands;

public class CreateItemHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly Mock<IItemTypeReader> _itemTypeReaderMock;
    private readonly Mock<ICategoryReader> _categoryReaderMock;
    private readonly CreateItemHandler _handler;

    public CreateItemHandlerTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _itemTypeReaderMock = new Mock<IItemTypeReader>();
        _categoryReaderMock = new Mock<ICategoryReader>();
        _handler = new CreateItemHandler(_itemRepositoryMock.Object, _itemTypeReaderMock.Object, _categoryReaderMock.Object);
    }

    [Fact]
    public async Task Handle_ValidItem_ReturnsCreatedItem()
    {
        // Arrange
        var newItemId = CatalogTestData.NextId();
        var ancestorCategories = CatalogTestData.CreateCategoryDtoAncestors();
        var category = CatalogTestData.CreateCategoryDto(ancestors: ancestorCategories);
        var itemType = CatalogTestData.CreateItemTypeDto();
        var command = CatalogTestData.GenerateCreateItemCommand(categoryId: category.Id, itemTypeId: itemType.Id);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(category);
        _itemTypeReaderMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync(itemType);
        _itemRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Item>()))
            .ReturnsAsync((Item item) =>
            {
                return Item.Reconstitute(newItemId, new ItemParameters(
                    command.Name,
                    command.Code,
                    command.CategoryId,
                    new Money(command.ListPrice.Amount, command.ListPrice.Currency),
                    command.ItemTypeId,
                    command.UnitOfMeasurement,
                    command.IsActive,
                    command.Description)
                );
            });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            Id = newItemId,
            Name = command.Name,
            Code = command.Code,
            Description = command.Description,
            IsActive = command.IsActive,
            UnitOfMeasurement = command.UnitOfMeasurement,
            ListPrice = new { Amount = command.ListPrice.Amount, Currency = command.ListPrice.Currency },
            CategoryId = command.CategoryId,
            ItemTypeId = command.ItemTypeId,
        });
        result.Category.Should().NotBeNull();
        result.Category?.Ancestors.Should().HaveCount(ancestorCategories.Count);
        result.ItemType.Should().NotBeNull();
        _categoryReaderMock.Verify(r => r.GetByIdAsync(command.CategoryId), Times.Once);
        _itemTypeReaderMock.Verify(r => r.GetByIdAsync(command.ItemTypeId), Times.Once);
        _itemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = CatalogTestData.GenerateCreateItemCommand();
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync((CategoryDto?)null);
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
        var category = CatalogTestData.CreateCategoryDto();
        var command = CatalogTestData.GenerateCreateItemCommand(category.Id);
        _categoryReaderMock.Setup(r => r.GetByIdAsync(command.CategoryId))
            .ReturnsAsync(category);
        _itemTypeReaderMock.Setup(r => r.GetByIdAsync(command.ItemTypeId))
            .ReturnsAsync((ItemTypeDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.ItemTypeId.ToString()));
        _itemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
    }
}
