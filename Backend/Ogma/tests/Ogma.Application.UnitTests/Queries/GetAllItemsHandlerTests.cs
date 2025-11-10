using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Queries;
public class GetAllItemsHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryStub;
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly GetAllItemsHandler _handler;

    public GetAllItemsHandlerTests()
    {
        _itemRepositoryStub = new Mock<IItemRepository>();
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new GetAllItemsHandler(_itemRepositoryStub.Object, _categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllItems()
    {
        // Arrange
        var ancestorCategories = new List<Category>
        {
            Category.Reconstitute(1L, "ParentCategory")
        };
        var childCategory = Category.Reconstitute(2L, "ChildCategory", 1L, "1");
        var itemType = ItemType.Reconstitute(1L, "ItemType", "");
        var items = new List<Item>
        {
            Item.Reconstitute(1L,
            new ItemParameters(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                childCategory,
                new Money(100m, "usd"),
                itemType,
                Guid.NewGuid().ToString()
                )
            ),
            Item.Reconstitute(2L,
                new ItemParameters(Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    childCategory,
                    new Money(200m, "usd"),
                    itemType,
                    Guid.NewGuid().ToString()
                )
            )
        };
        _itemRepositoryStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items);
        _categoryRepositoryStub.Setup(r => r.GetAncestorsAsync(items[0].Category.Id))
            .ReturnsAsync(ancestorCategories);
        _categoryRepositoryStub.Setup(r => r.GetAncestorsAsync(items[1].Category.Id))
            .ReturnsAsync(ancestorCategories);
        // Act
        var result = await _handler.Handle(new GetAllItemsQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Id.Should().Be(1L);
        result[1].Id.Should().Be(2L);
        _itemRepositoryStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]  
    public async Task Handle_NoItems_RetunrsEmtpyList()
    {
        // Arrange
        var items = new List<Item>();
        _itemRepositoryStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items);
        // Act
        var result = await _handler.Handle(new GetAllItemsQuery(), CancellationToken.None);
        // Assert
        result.Count.Should().Be(0);
        _itemRepositoryStub.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
