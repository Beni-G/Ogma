namespace Ogma.Api.Contracts.Orders;

public record CreateOrderRequest(
    long PartnerId,
    string OrderNumber,
    DateTime OrderDate,
    long OrderTypeId,
    long OrderStatusId,
    string? AdditionalInformation,
    List<CreateOrderLineRequest> OrderLines);

