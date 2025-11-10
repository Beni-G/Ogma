using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Queries;
public class GetItemByIdHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryStub;
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly GetItemByIdHandler _handler;

    public GetItemByIdHandlerTests()
    {
        _itemRepositoryStub = new Mock<IItemRepository>();
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new GetItemByIdHandler(_itemRepositoryStub.Object, _categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsItem()
    {
        // Arrange
        long id = 1L;
        var category = Category.Reconstitute(1L, "Category");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "ItemType Description");
        var item = Item.Reconstitute(id, new ItemParameters(
            "ItemName",
            "ItemCode",
            category,
            new Money(100m, "eur"),
            itemType,
            "box",
            true,
            "ItemDescription"
            ));
        _itemRepositoryStub.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(item);
        _categoryRepositoryStub.Setup(r => r.GetAncestorsAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<Category>());
        // Act
        var result = await _handler.Handle(new GetItemByIdQuery(id), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);  
        result.Name.Should().Be(item.Name);
        result.Code.Should().Be(item.Code);
        result.Category.Id.Should().Be(item.Category.Id);
        result.ItemType.Id.Should().Be(item.ItemType.Id);
        _categoryRepositoryStub.Verify(r => r.GetAncestorsAsync(It.IsAny<long>()), Times.Once);
        _itemRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var query = new GetItemByIdQuery(id);
        _itemRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Item?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _itemRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
