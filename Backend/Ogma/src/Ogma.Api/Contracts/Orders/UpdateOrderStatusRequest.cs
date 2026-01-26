namespace Ogma.Api.Contracts.Orders;

public record UpdateOrderStatusRequest(long Id, string Name, string Description);

