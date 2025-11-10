using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Repositories;
using FluentAssertions;
using Ogma.Domain.Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Commands;
public class DeleteItemHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly DeleteItemHandler _handler;

    public DeleteItemHandlerTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _handler = new DeleteItemHandler(_itemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingItem_DeletesItem()
    {
        // Arrange
        var itemParameters = new ItemParameters(
            Name: "Test Item",
            Code: "TI001",
            Category: Category.Reconstitute(1L, "Test Category"),
            ListPrice: new Money(100m, "USD"),
            ItemType: ItemType.Reconstitute(1L, "Test ItemType", ""),
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A test item"
        );
        var item = Item.Reconstitute(1L, itemParameters);
        var command = new DeleteItemCommand(item.Id);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(item.Id))
            .ReturnsAsync(item);
        _itemRepositoryMock.Setup(r => r.DeleteAsync(item))
            .Returns(Task.CompletedTask);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        _itemRepositoryMock.Verify(r => r.GetByIdAsync(item.Id), Times.Once);
        _itemRepositoryMock.Verify(r => r.DeleteAsync(item), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingItem_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteItemCommand(1L);
        _itemRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Item?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _itemRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _itemRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Item>()), Times.Never);
    }
}
