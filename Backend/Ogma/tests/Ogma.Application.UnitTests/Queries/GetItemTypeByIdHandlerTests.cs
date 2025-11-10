using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Application.UnitTests.Queries;
public class GetItemTypeByIdHandlerTests
{

    private readonly Mock<IItemTypeRepository> _itemTypeRepositoryStub;
    private readonly GetItemTypeByIdHandler _handler;


    public GetItemTypeByIdHandlerTests()
    {
        _itemTypeRepositoryStub = new Mock<IItemTypeRepository>();
        _handler = new GetItemTypeByIdHandler(_itemTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsItemType()
    {
        // Arrange
        long id = 1;
        var existingItemType = ItemType.Reconstitute(id, "ExistingName", "ExistingDescription");
        var query = new GetItemTypeByIdQuery(id);
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingItemType);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be(existingItemType.Name);
        result.Description.Should().Be(existingItemType.Description);
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var query = new GetItemTypeByIdQuery(id);
        _itemTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ItemType?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _itemTypeRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
