namespace Ogma.Api.Contracts.Orders;

public record OrderResponse(
    long Id,
    OrderPartnerResponse OrderPartner,
    string OrderNumber,
    DateTime OrderDate,
    long OrderTypeId,
    OrderTypeResponse OrderType,
    long OrderStatusId,
    OrderStatusResponse OrderStatus,
    string? AdditionalInformation,
    IReadOnlyCollection<OrderLineResponse> OrderLines);