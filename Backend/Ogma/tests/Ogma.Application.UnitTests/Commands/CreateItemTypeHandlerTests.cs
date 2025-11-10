using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Commands;
public class CreateItemTypeHandlerTests
{
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly CreateItemTypeHandler _handler;

    public CreateItemTypeHandlerTests()
    {
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _handler = new CreateItemTypeHandler(_itemTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidItemType_ReturnsCreatedItemType()
    {
        // Arrange
        var command = new CreateItemTypeCommand(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        long nextId = 1;

        _itemTypeRepositoryStub.Setup(repo => repo.AddAsync(It.IsAny<ItemType>()))
            .ReturnsAsync((ItemType itemType) =>
            {
                return ItemType.Reconstitute(nextId, itemType.Name, itemType.Description);
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(nextId);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        _itemTypeRepositoryStub.Verify(r => r.AddAsync(It.IsAny<ItemType>()), Times.Once);
    }
}
