using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Application.UnitTests.Catalog.Tests.Commands;

public class DeleteItemTypeHandlerTests
{
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly DeleteItemTypeHandler _handler;
    private readonly EntityMetadata _metadata;


    public DeleteItemTypeHandlerTests()
    {
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _handler = new DeleteItemTypeHandler(_itemTypeRepositoryStub.Object);
        _metadata = new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
    }

    [Fact]
    public async Task Handle_ValidItemType_DeletesItemType()
    {
        // Arrange
        long id = 1;
        var existingItemType = ItemType.Reconstitute(id, "ExistingName", "ExistingDescription", _metadata);
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
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
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
