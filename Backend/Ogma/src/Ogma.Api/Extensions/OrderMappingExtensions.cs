using Ogma.Api.Contracts.Orders;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Api.Extensions;

public static class OrderMappingExtensions
{
    public static OrderTypeResponse ToResponse(this OrderTypeDto orderType) => new(orderType.Id, orderType.Code, orderType.Description);

    public static OrderStatusResponse ToResponse(this OrderStatusDto orderStatus) => new(orderStatus.Id, orderStatus.Name, orderStatus.Description);

    public static OrderPartnerResponse ToResponse(this OrderPartnerDto orderPartner) => new(orderPartner.PartnerId, orderPartner.PartnerName);

    public static OrderItemResponse ToResponse(this OrderItemDto orderItem) => new(orderItem.ItemId, orderItem.ItemName, orderItem.ItemCode);

    public static OrderLineResponse ToResponse(this OrderLineDto orderLine) => new(
        orderLine.Id,
        orderLine.OrderItem!.ToResponse(),
        orderLine.OrderedQuantity,
        orderLine.CancelledQuantity,
        orderLine.FullfilledQuantity,
        orderLine.Price.ToResponse(),
        orderLine.ExchangeRate?.ToResponse(),
        orderLine.AdditionalInformation
    );

    public static OrderResponse ToResponse(this OrderDto order) => new(
        order.Id,
        order.OrderPartner.ToResponse(),
        order.OrderNumber,
        order.OrderDate,
        order.OrderTypeId,
        order.OrderType!.ToResponse(),
        order.OrderStatusId,
        order.OrderStatus!.ToResponse(),
        order.AdditionalInformation,
        order.OrderLines.Select(ol => ol.ToResponse()).ToList()
    );
}
