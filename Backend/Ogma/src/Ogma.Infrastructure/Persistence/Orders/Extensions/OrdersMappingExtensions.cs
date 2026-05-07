using Ogma.Application.Orders.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Infrastructure.Persistence.SharedKernel.Extensions;

namespace Ogma.Infrastructure.Persistence.Orders.Extensions;

public static class OrdersMappingExtensions
{
    #region From Persistence Models ToDto Methods

    public static OrderItemDto ToDto(this ValueObjectRecords.OrderItem orderItem) =>
        new(orderItem.ItemId, orderItem.ItemName, orderItem.ItemCode);

    public static OrderPartnerDto ToDto(this ValueObjectRecords.OrderPartner orderPartner) =>
        new(orderPartner.PartnerId, orderPartner.PartnerName);

    public static OrderTypeDto ToDto(this Models.OrderType orderType) =>
        new(orderType.Id, orderType.Code, orderType.Description);

    public static OrderStatusDto ToDto(this Models.OrderStatus orderStatus) =>
        new(orderStatus.Id, orderStatus.Name, orderStatus.Description);

    public static OrderLineDto ToDto(this Models.OrderLine orderLine) =>
        new OrderLineDto(orderLine.Id,
            orderLine.OrderItem.ToDto(),
            orderLine.OrderedQuantity,
            orderLine.CancelledQuantity,
            orderLine.FullfilledQuantity,
            new MoneyDto(orderLine.PriceAmount, orderLine.PriceCurrency),
            new ExchangeRateDto(orderLine.PriceCurrency, orderLine.ExchangeTargetCurrency, orderLine.ExchangeRate),
            orderLine.AdditionalInformation!);

    public static OrderDto ToDto(this Models.Order order) =>
        new OrderDto(order.Id,
            order.OrderPartner.ToDto(),
            order.OrderNumber,
            order.OrderDate,
            order.OrderTypeId,
            order.OrderType.ToDto(),
            order.OrderStatusId,
            order.OrderStatus.ToDto(),
            order.AdditionalInformation!,
            order.OrderLines.Select(ol => ol.ToDto()).ToList());

    #endregion

    #region From Persistence Models ToDomain Methods

    public static OrderType ToDomain(this Models.OrderType orderType) =>
        OrderType.Reconstitute(orderType.Id, orderType.Code, orderType.Description, new EntityMetadata(orderType.CreatedAt, orderType.UpdatedAt, orderType.Version));

    public static OrderStatus ToDomain(this Models.OrderStatus orderStatus) =>
        OrderStatus.Reconstitute(orderStatus.Id, orderStatus.Name, orderStatus.Description, new EntityMetadata(orderStatus.CreatedAt, orderStatus.UpdatedAt, orderStatus.Version));

    public static OrderLine ToDomain(this Models.OrderLine orderLine) =>
        OrderLine.Reconstitute(
            orderLine.Id,
            new OrderItem(
                orderLine.OrderItem.ItemId,
                orderLine.OrderItem.ItemName,
                orderLine.OrderItem.ItemCode),
            orderLine.OrderedQuantity,
            orderLine.CancelledQuantity,
            orderLine.FullfilledQuantity,
            new Money(orderLine.PriceAmount, orderLine.PriceCurrency),
            new EntityMetadata(orderLine.CreatedAt, orderLine.UpdatedAt, orderLine.Version),
            !string.IsNullOrWhiteSpace(orderLine.ExchangeTargetCurrency)
                ? new ExchangeRate(
                    orderLine.PriceCurrency,
                    orderLine.ExchangeTargetCurrency!,
                    orderLine.ExchangeRate)
                : null,
            orderLine.AdditionalInformation!);

    public static Order ToDomain(this Models.Order order) =>
        Order.Reconstitute(
            order.Id,
            new OrderPartner(order.OrderPartner.PartnerId, order.OrderPartner.PartnerName),
            order.OrderNumber,
            order.OrderDate,
            order.OrderTypeId,
            order.OrderStatusId,
            new EntityMetadata(order.CreatedAt, order.UpdatedAt, order.Version),
            order.AdditionalInformation!,
            order.OrderLines.Select(ol => ol.ToDomain()).ToList());

    #endregion

    #region From Domain ToModels Methods

    public static ValueObjectRecords.OrderItem ToModel(this OrderItem orderItem) =>
        new(orderItem.ItemId, orderItem.ItemName, orderItem.ItemCode);

    public static ValueObjectRecords.OrderPartner ToModel(this OrderPartner orderPartner) =>
        new(orderPartner.PartnerId, orderPartner.PartnerName);

    public static Models.OrderType ToModel(this OrderType orderType)
    {
        var modelOrderType = new Models.OrderType
        {
            Code = orderType.Code,
            Description = orderType.Description
        };
        orderType.MapBaseProperties(modelOrderType);
        return modelOrderType;
    }


    public static Models.OrderStatus ToModel(this OrderStatus orderStatus)
    {
        var modelOrderStatus = new Models.OrderStatus
        {
            Name = orderStatus.Name,
            Description = orderStatus.Description
        };
        orderStatus.MapBaseProperties(modelOrderStatus);
        return modelOrderStatus;
    }

    public static Models.OrderLine ToModel(this OrderLine orderLine)
    {
        var modelOrderLine = new Models.OrderLine
        {
            OrderItem = orderLine.OrderItem.ToModel(),
            OrderedQuantity = orderLine.OrderedQuantity,
            CancelledQuantity = orderLine.CancelledQuantity,
            FullfilledQuantity = orderLine.FullfilledQuantity,
            PriceAmount = orderLine.Price.Amount,
            PriceCurrency = orderLine.Price.Currency,
            ExchangeRate = orderLine.ExchangeRate?.Rate ?? 0,
            ExchangeTargetCurrency = orderLine.ExchangeRate?.TargetCurrency ?? string.Empty,
            AdditionalInformation = orderLine.AdditionalInformation
        };
        orderLine.MapBaseProperties(modelOrderLine);
        return modelOrderLine;
    }

    public static Models.Order ToModel(this Order order)
    {
        var modelOrder = new Models.Order
        {
            OrderPartner = order.OrderPartner.ToModel(),
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            OrderTypeId = order.OrderTypeId,
            OrderStatusId = order.OrderStatusId,
            AdditionalInformation = order.AdditionalInformation,
            OrderLines = order.OrderLines.Select(ol => ol.ToModel()).ToList()
        };
        order.MapBaseProperties(modelOrder);
        return modelOrder;
    }

    #endregion
}
