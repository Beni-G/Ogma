using Ogma.Application.Orders.Dtos;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;

namespace Ogma.Application.Orders.Extensions;

public static class OrdersMappingExtensions
{
    #region ToDto Methods

    public static OrderTypeDto ToDto(this OrderType orderType) => new(orderType.Id, orderType.Code, orderType.Description);

    public static OrderStatusDto ToDto(this OrderStatus orderStatus) => new(orderStatus.Id, orderStatus.Name, orderStatus.Description);

    public static OrderPartnerDto ToDto(this OrderPartner orderPartner) => new(orderPartner.PartnerId, orderPartner.PartnerName);

    public static OrderItemDto ToDto(this OrderItem orderItem) => new OrderItemDto(orderItem.ItemId, orderItem.ItemName, orderItem.ItemCode);

    public static OrderLineDto ToDto(this OrderLine orderLine) => new OrderLineDto(
        orderLine.Id,
        orderLine.OrderItem.ToDto(),
        orderLine.OrderedQuantity,
        orderLine.CancelledQuantity,
        orderLine.FullfilledQuantity,
        orderLine.Price.ToDto(),
        orderLine.ExchangeRate is not null
            ? orderLine.ExchangeRate.ToDto()
            : null!,
        orderLine.AdditionalInformation
    );

    public static OrderDto ToDtoWithDomain(this Order order, OrderType? orderType = null, OrderStatus? orderStatus = null) => new(
        order.Id,
        order.OrderPartner.ToDto(),
        order.OrderNumber,
        order.OrderDate,
        order.OrderTypeId,
        orderType?.ToDto(),
        order.OrderStatusId,
        orderStatus?.ToDto(),
        order.AdditionalInformation,
        order.OrderLines.Select(ol => ol.ToDto()).ToList()
    );

    public static OrderDto ToDtoWithDto(this Order order, OrderTypeDto? orderType = null, OrderStatusDto? orderStatus = null) => new(
        order.Id,
        order.OrderPartner.ToDto(),
        order.OrderNumber,
        order.OrderDate,
        order.OrderTypeId,
        orderType,
        order.OrderStatusId,
        orderStatus,
        order.AdditionalInformation,
        order.OrderLines.Select(ol => ol.ToDto()).ToList()
    );

    #endregion

    #region ToDomain

    public static OrderType ToDomain(this OrderTypeDto orderTypeDto) => OrderType.Reconstitute(orderTypeDto.Id, orderTypeDto.Code, orderTypeDto.Description);

    public static OrderStatus ToDomain(this OrderStatusDto orderStatusDto) => OrderStatus.Reconstitute(orderStatusDto.Id, orderStatusDto.Name, orderStatusDto.Description);

    public static OrderPartner ToDomain(this OrderPartnerDto orderPartnerDto) => new(orderPartnerDto.PartnerId, orderPartnerDto.PartnerName);

    public static OrderItem ToDomain(this OrderItemDto orderItemDto) => new(orderItemDto.ItemId, orderItemDto.ItemName, orderItemDto.ItemName);

    public static OrderLine ToDomain(this OrderLineDto orderLineDto) =>
        OrderLine.Reconstitute(
            orderLineDto.Id,
            orderLineDto.OrderItem!.ToDomain(),
            orderLineDto.OrderedQuantity,
            orderLineDto.CancelledQuantity,
            orderLineDto.FullfilledQuantity,
            orderLineDto.Price.ToDomain(),
            orderLineDto.ExchangeRate?.ToDomain(),
            orderLineDto.AdditionalInformation
        );

    public static Order ToDomain(this OrderDto orderDto) =>
        Order.Reconstitute(
            orderDto.Id,
            orderDto.OrderPartner.ToDomain(),
            orderDto.OrderNumber,
            orderDto.OrderDate,
            orderDto.OrderTypeId,
            orderDto.OrderStatusId,
            orderDto.AdditionalInformation,
            orderDto.OrderLines.Select(ol => ol.ToDomain()).ToList()
        );

    #endregion
}
