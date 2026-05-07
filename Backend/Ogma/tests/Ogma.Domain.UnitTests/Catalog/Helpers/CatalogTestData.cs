using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Catalog.Helpers;

internal static class CatalogTestData
{
    public static long NextId() => Random.Shared.NextInt64(1, long.MaxValue);

    public static ItemParameters CreateItemParameters() => new(
            Name: CreateName(),
            Code: CreateCode(),
            CategoryId: NextId(),
            ListPrice: CreateListPrice(),
            ItemTypeId: NextId(),
            UnitOfMeasurement: CreateUnitOfMeasurement(),
            IsActive: CreateIsActive(),
            Description: CreateDescription()
    );

    public static string CreateDescription()
    {
        return $"Description_{Guid.NewGuid().ToString()[..20]}";
    }

    public static string CreateUnitOfMeasurement()
    {
        return $"UoM_{Guid.NewGuid().ToString()[..5]}";
    }

    public static Money CreateListPrice()
    {
        return new Money(99.99m, "USD");
    }

    public static string CreateCode()
    {
        return $"Code_{Guid.NewGuid().ToString()[..8]}";
    }

    public static string CreateName()
    {
        return $"Name_{Guid.NewGuid().ToString()[..8]}";
    }

    public static bool CreateIsActive() => Random.Shared.Next(0, 2) == 1;

    public static Item CreateItem() => Item.Create(CreateItemParameters());

    public static EntityMetadata GetMetadata() => new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
}
