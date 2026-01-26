namespace Ogma.Api.Contracts.Orders;

public record UpdateOrderTypeRequest(long Id, string Code, string Description);
