using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Queries;
public class GetAllItemTypesHandlerTests
{
    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly GetAllItemTypesHandler _handler;


    public GetAllItemTypesHandlerTests()
    {
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _handler = new GetAllItemTypesHandler(_itemTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllItemTypes()
    {
        // Arrange
        var itemTypes = new List<ItemType>
        {
            ItemType.Reconstitute(1, "Type1", "Description1"),
            ItemType.Reconstitute(2, "Type2", "Description2"),
        };
        _itemTypeRepositoryStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(itemTypes);
        // Act
        var result = await _handler.Handle(new GetAllItemTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Type1");
        result[0].Description.Should().Be("Description1");
        result[1].Id.Should().Be(2);
        result[1].Name.Should().Be("Type2");
        result[1].Description.Should().Be("Description2");
        _itemTypeRepositoryStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoItemTypes_ReturnsEmptyList()
    {
        // Arrange
        var itemTypes = new List<ItemType>();
        _itemTypeRepositoryStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(itemTypes);
        // Act
        var result = await _handler.Handle(new GetAllItemTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
        _itemTypeRepositoryStub.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
