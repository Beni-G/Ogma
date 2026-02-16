using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.UnitTests.Catalog.Helpers;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Catalog.Tests.Commands;

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
        var itemParameters = CatalogTestData.CreateItemParameters();
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
