using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Application.UnitTests.Orders.Helpers;

internal static class OrdersTestData
{
    public static EntityMetadata GetMetadata() => new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
}
