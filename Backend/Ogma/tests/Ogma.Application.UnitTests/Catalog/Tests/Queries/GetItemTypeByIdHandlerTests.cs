using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;

public class GetItemTypeByIdHandlerTests
{

    private readonly Mock<IItemTypeReader> _itemTypeReaderStub;
    private readonly GetItemTypeByIdHandler _handler;

    public GetItemTypeByIdHandlerTests()
    {
        _itemTypeReaderStub = new Mock<IItemTypeReader>();
        _handler = new GetItemTypeByIdHandler(_itemTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsItemType()
    {
        // Arrange
        var existingItemType = CatalogTestData.CreateItemTypeDto();
        var query = new GetItemTypeByIdQuery(existingItemType.Id);
        _itemTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingItemType);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(existingItemType);
        _itemTypeReaderStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var query = new GetItemTypeByIdQuery(id);
        _itemTypeReaderStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ItemTypeDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _itemTypeReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
