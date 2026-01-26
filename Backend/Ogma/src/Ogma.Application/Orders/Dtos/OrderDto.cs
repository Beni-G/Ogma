namespace Ogma.Application.Orders.Dtos;

public record OrderDto(
    long Id,
    OrderPartnerDto OrderPartner,
    string OrderNumber,
    DateTime OrderDate,
    long OrderTypeId,
    OrderTypeDto? OrderType,
    long OrderStatusId,
    OrderStatusDto? OrderStatus,
    string? AdditionalInformation,
    IReadOnlyCollection<OrderLineDto> OrderLines
    );
