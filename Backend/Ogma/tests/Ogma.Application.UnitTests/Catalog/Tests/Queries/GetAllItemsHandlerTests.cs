using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.UnitTests.Catalog.Helpers;

namespace Ogma.Application.UnitTests.Catalog.Tests.Queries;

public class GetAllItemsHandlerTests
{
    private readonly Mock<IItemReader> _itemReaderStub;
    private readonly Mock<ICategoryReader> _categoryReaderStub;
    private readonly GetAllItemsHandler _handler;

    public GetAllItemsHandlerTests()
    {
        _itemReaderStub = new Mock<IItemReader>();
        _categoryReaderStub = new Mock<ICategoryReader>();
        _handler = new GetAllItemsHandler(_itemReaderStub.Object, _categoryReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllItems()
    {
        // Arrange
        var items = new List<ItemDto>();
        var ancestors1 = CatalogTestData.CreateCategoryDtoAncestors();
        var category1 = CatalogTestData.CreateCategoryDto(ancestors1);
        var item1NoCategory = CatalogTestData.CreateItemDto(categoryId: category1.Id);
        var item1 = item1NoCategory with { Category = category1 };
        items.Add(item1);
        var ancestors2 = CatalogTestData.CreateCategoryDtoAncestors();
        var category2 = CatalogTestData.CreateCategoryDto(ancestors2);
        var item2NoCategory = CatalogTestData.CreateItemDto(categoryId: category2.Id);
        var item2 = item2NoCategory with { Category = category2 };
        items.Add(item2);
        var categoryIds = items.Select(i => i.CategoryId).Distinct().ToList();
        var ancestorsLookup = new Dictionary<long, IEnumerable<CategoryDto>>
        {
            { category1.Id, ancestors1 },
            { category2.Id, ancestors2 }
        };
        var categoriesLookup = new Dictionary<long, CategoryDto>
        {
            { category1.Id, category1 },
            { category2.Id, category2 }
        };
        _itemReaderStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items);
        _categoryReaderStub.Setup(c => c.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(categoriesLookup);
        _categoryReaderStub.Setup(r => r.GetAncestorsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(ancestorsLookup);
        // Act
        var result = await _handler.Handle(new GetAllItemsQuery(), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Id.Should().Be(item1.Id);
        result[0].Category.Should().NotBeNull();
        result[0].Category!.Ancestors.Should().BeEquivalentTo(ancestors1);
        result[0].Category!.Ancestors!.Count.Should().Be(ancestors1.Count);
        result[1].Id.Should().Be(item2.Id);
        result[1].Category.Should().NotBeNull();
        result[1].Category!.Ancestors.Should().BeEquivalentTo(ancestors2);
        result[1].Category!.Ancestors!.Count.Should().Be(ancestors2.Count);
        _itemReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
        _categoryReaderStub.Verify(c => c.GetByIdsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);
        _categoryReaderStub.Verify(r => r.GetAncestorsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NoItems_RetunrsEmtpyList()
    {
        // Arrange
        var items = new List<ItemDto>();
        _categoryReaderStub.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(new Dictionary<long, CategoryDto>());
        _itemReaderStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items);
        // Act
        var result = await _handler.Handle(new GetAllItemsQuery(), CancellationToken.None);
        // Assert
        result.Count.Should().Be(0);
        _itemReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ThrwosKeyNotFoundException()
    {
        // Arrange
        var items = new List<ItemDto>();
        var ancestors1 = CatalogTestData.CreateCategoryDtoAncestors();
        var category1 = CatalogTestData.CreateCategoryDto(ancestors1);
        var item1NoCategory = CatalogTestData.CreateItemDto(categoryId: category1.Id);
        var item1 = item1NoCategory with { Category = category1 };
        items.Add(item1);
        var ancestors2 = CatalogTestData.CreateCategoryDtoAncestors();
        var category2 = CatalogTestData.CreateCategoryDto(ancestors2);
        var item2NoCategory = CatalogTestData.CreateItemDto(categoryId: category2.Id);
        var item2 = item2NoCategory with { Category = category2 };
        items.Add(item2);
        var categoryIds = items.Select(i => i.CategoryId).Distinct().ToList();
        var ancestorsLookup = new Dictionary<long, IEnumerable<CategoryDto>>
        {
            { category1.Id, ancestors1 },
            { category2.Id, ancestors2 }
        };
        var categoriesLookup = new Dictionary<long, CategoryDto>
        {
            { category1.Id, category1 }
        };
        _itemReaderStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items);
        _categoryReaderStub.Setup(c => c.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(categoriesLookup);
        _categoryReaderStub.Setup(r => r.GetAncestorsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(ancestorsLookup);
        // Act
        Func<Task> act = async () => await _handler.Handle(new GetAllItemsQuery(), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(item2.CategoryId.ToString()));
    }

    [Fact]
    public async Task Handle_CategoryWithoutAncestors_ReturnsEmptyList()
    {
        // Arrange
        var items = new List<ItemDto>();
        var category = CatalogTestData.CreateCategoryDto();
        var itemNoCategory = CatalogTestData.CreateItemDto(categoryId: category.Id);
        var item = itemNoCategory with { Category = category };
        items.Add(item);
        var categoryIds = items.Select(i => i.CategoryId).Distinct().ToList();
        var ancestorsLookup = new Dictionary<long, IEnumerable<CategoryDto>>
        {
            { category.Id, new List<CategoryDto>() }
        };
        var categoriesLookup = new Dictionary<long, CategoryDto>
        {
            { category.Id, category }
        };
        _itemReaderStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items);
        _categoryReaderStub.Setup(c => c.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(categoriesLookup);
        _categoryReaderStub.Setup(r => r.GetAncestorsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(ancestorsLookup);
        // Act
        var result = await _handler.Handle(new GetAllItemsQuery(), CancellationToken.None);
        // Assert
        result[0].Should().NotBeNull();
        result[0].Category!.Ancestors.Should().BeEmpty();
    }
}
