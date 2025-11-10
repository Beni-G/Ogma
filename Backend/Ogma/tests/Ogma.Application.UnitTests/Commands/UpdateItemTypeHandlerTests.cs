using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Commands;
public class UpdateItemTypeHandlerTests
{
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly UpdateItemTypeHandler _handler;


    public UpdateItemTypeHandlerTests()
    {
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _handler = new UpdateItemTypeHandler(_itemTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidItemType_ReturnsUpdatedItemType()
    {
        // Arrange
        long id = 1;
        var existingItemType = ItemType.Reconstitute(id, "ExistingName", "ExistingDescription");
        var command = new UpdateItemTypeCommand(id, "UpdatedName", "UpdatedDescription");
        _itemTypeRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<ItemType>()))
            .ReturnsAsync((ItemType itemType) => true);
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingItemType);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _itemTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<ItemType>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingItemType_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var command = new UpdateItemTypeCommand(id, "UpdatedName", "UpdatedDescription");
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ItemType?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString())); ;
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _itemTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<ItemType>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ReturnsInvalidOperationException()
    {
        // Arrange
        long id = 1;
        var existingItemType = ItemType.Reconstitute(id, "ExistingName", "ExistingDescription");
        var command = new UpdateItemTypeCommand(id, "UpdatedName", "UpdatedDescription");
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingItemType);
        _itemTypeRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<ItemType>()))
            .ReturnsAsync(false);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _itemTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<ItemType>()), Times.Once);
    }

}
