using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Application.UnitTests.Commands;
public class DeleteItemTypeHandlerTests
{
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly DeleteItemTypeHandler _handler;


    public DeleteItemTypeHandlerTests()
    {
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _handler = new DeleteItemTypeHandler(_itemTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidItemType_ReturnsUpdatedItemType()
    {
        // Arrange
        long id = 1;
        var existingItemType = ItemType.Reconstitute(id, "ExistingName", "ExistingDescription");
        var command = new DeleteItemTypeCommand(id);
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingItemType);
        _itemTypeRepositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<ItemType>()))
            .Returns(Task.CompletedTask);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _itemTypeRepositoryStub.Verify(r => r.DeleteAsync(existingItemType), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var command = new DeleteItemTypeCommand(id);
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ItemType?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _itemTypeRepositoryStub.Verify(r => r.DeleteAsync(It.IsAny<ItemType>()), Times.Never);
    }
}
