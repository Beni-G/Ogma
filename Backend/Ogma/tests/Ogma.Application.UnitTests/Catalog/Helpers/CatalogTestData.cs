using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Catalog.Helpers;

internal static class CatalogTestData
{
    public static long NextId() => Random.Shared.NextInt64(1, long.MaxValue);

    public static string CreateName() => $"Name_{Guid.NewGuid().ToString()[..8]}";

    public static string CreateCode() => $"Code_{Guid.NewGuid().ToString()[..8]}";

    public static string CreateDescription() => $"Description_{Guid.NewGuid().ToString()[..20]}";

    public static string CreateUnitOfMeasurement() => $"UoM_{Guid.NewGuid().ToString()[..5]}";

    public static MoneyDto CreateListPriceAmount() => new(Math.Round((decimal)(Random.Shared.NextDouble() * 1000), 2), "EUR");

    public static bool CreateIsActive() => Random.Shared.Next(0, 2) == 1;

    public static CreateItemCommand GenerateCreateItemCommand(long categoryId = 0, long itemTypeId = 0) => new(
        Name: CreateName(),
        Code: CreateCode(),
        CategoryId: categoryId == 0 ? NextId() : categoryId,
        ListPrice: CreateListPriceAmount(),
        ItemTypeId: itemTypeId == 0 ? NextId() : itemTypeId,
        UnitOfMeasurement: CreateUnitOfMeasurement(),
        IsActive: CreateIsActive(),
        Description: CreateDescription()
    );

    public static UpdateItemCommand GenerateUpdateItemCommand(long id, long categoryId = 0, long itemTypeId = 0) => new(
        Id: id,
        Name: CreateName(),
        Code: CreateCode(),
        CategoryId: categoryId == 0 ? NextId() : categoryId,
        ListPrice: CreateListPriceAmount(),
        ItemTypeId: itemTypeId == 0 ? NextId() : itemTypeId,
        UnitOfMeasurement: CreateUnitOfMeasurement(),
        IsActive: CreateIsActive(),
        Description: CreateDescription()
    );

    public static List<CategoryDto> CreateCategoryDtoAncestors()
    {
        var ancestors = new List<CategoryDto>();
        var levels = Category.MaxDepth;
        long? parentId = null;
        string? path = null;
        for (int i = 1; i < levels; i++)
        {
            var ancestor = new CategoryDto(
                Id: NextId(),
                Name: CreateName(),
                ParentCategoryId: parentId,
                Path: path);
            ancestors.Add(ancestor);
            parentId = ancestor.Id;
            path = path is null ? $"{ancestor.Id}" : $"{path}/{ancestor.Id}";
        }
        return ancestors;
    }

    public static CategoryDto CreateCategoryDto(IEnumerable<CategoryDto>? ancestors = null)
    {
        var lastAncestor = ancestors?.LastOrDefault();
        return new CategoryDto(
            Id: NextId(),
            Name: CreateName(),
            ParentCategoryId: lastAncestor is null ? null : lastAncestor.Id,
            Path: ancestors is null ? null : $"{lastAncestor!.Path}/{lastAncestor.Id}",
            Ancestors: ancestors is null ? null : ancestors.ToList().AsReadOnly()
            );
    }

    public static ItemTypeDto CreateItemTypeDto() => new(
        Id: NextId(),
        Name: CreateName(),
        Description: CreateDescription()
    );

    public static ItemDto CreateItemDto(long? categoryId = null, long? itemTypeId = null) => new(
            Id: NextId(),
            Name: CreateName(),
            Code: CreateCode(),
            Description: CreateDescription(),
            CategoryId: categoryId ?? NextId(),
            Category: CreateCategoryDto(),
            ListPrice: CreateListPriceAmount(),
            ItemTypeId: itemTypeId ?? NextId(),
            ItemType: CreateItemTypeDto(),
            UnitOfMeasurement: CreateUnitOfMeasurement(),
            IsActive: CreateIsActive()
    );

    public static ItemParameters CreateItemParameters(long? categoryId = null, long? itemTypeId = null) => new(
            Name: $"Name_{Guid.NewGuid().ToString()[..8]}",
            Code: $"Code_{Guid.NewGuid().ToString()[..8]}",
            CategoryId: categoryId ?? NextId(),
            ListPrice: new Money(99.99m, "USD"),
            ItemTypeId: itemTypeId ?? NextId(),
            UnitOfMeasurement: $"UoM_{Guid.NewGuid().ToString()[..5]}",
            IsActive: false,
            Description: $"Description_{Guid.NewGuid().ToString()[..20]}"
    );

    public static Item CreateItem() => Item.Create(CreateItemParameters());

}
