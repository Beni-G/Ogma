using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;

public class GetAllItemTypesHandlerTests
{
    private readonly Mock<IItemTypeReader> _itemTypeReaderStub;
    private readonly GetAllItemTypesHandler _handler;

    public GetAllItemTypesHandlerTests()
    {
        _itemTypeReaderStub = new Mock<IItemTypeReader>();
        _handler = new GetAllItemTypesHandler(_itemTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllItemTypes()
    {
        // Arrange
        var itemTypes = new List<ItemTypeDto>
        {
            CatalogTestData.CreateItemTypeDto(),
            CatalogTestData.CreateItemTypeDto()
        };
        _itemTypeReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(itemTypes);
        // Act
        var result = await _handler.Handle(new GetAllItemTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(itemTypes, options => options.WithStrictOrdering());
        _itemTypeReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoItemTypes_ReturnsEmptyList()
    {
        // Arrange
        var itemTypes = new List<ItemTypeDto>();
        _itemTypeReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(itemTypes);
        // Act
        var result = await _handler.Handle(new GetAllItemTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        _itemTypeReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
