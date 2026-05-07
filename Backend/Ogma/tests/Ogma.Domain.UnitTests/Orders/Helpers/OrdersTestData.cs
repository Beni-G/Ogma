using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.UnitTests.Orders.Helpers;

internal static class OrdersTestData
{
    public static long NextId() => Random.Shared.NextInt64(1, long.MaxValue);

    public static OrderPartner CreateOrderPartner(long? id = null, string? name = null)
    {
        return new OrderPartner(
            id ?? Random.Shared.NextInt64(1, long.MaxValue),
            name ?? $"Partner_{Guid.NewGuid().ToString()[..8]}"
        );
    }

    public static OrderItem CreateOrderItem(long? id = null, string? name = null, string? code = null)
    {
        return new OrderItem(
            id ?? Random.Shared.NextInt64(1, long.MaxValue),
            name ?? Guid.NewGuid().ToString(),
            code ?? Guid.NewGuid().ToString()
            );
    }

    public static EntityMetadata GetMetadata() => new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
}
