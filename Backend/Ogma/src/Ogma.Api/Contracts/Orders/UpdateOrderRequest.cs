namespace Ogma.Api.Contracts.Orders;

public record UpdateOrderRequest(
    long Id,
    long PartnerId,
    string OrderNumber,
    DateTime OrderDate,
    long OrderTypeId,
    long OrderStatusId,
    string? AdditionalInformation,
    List<UpdateOrderLineRequest> OrderLines
);
