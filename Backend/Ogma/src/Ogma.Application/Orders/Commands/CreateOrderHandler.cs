using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Application.Orders.Ports;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderTypeReader _orderTypeReader;
    private readonly IOrderStatusReader _orderStatusReader;
    private readonly ICatalogItemReader _catalogItemReader;
    private readonly IPartnerReader _partnerReader;

    public CreateOrderHandler(
        IOrderRepository orderRepository,
        IOrderTypeReader orderTypeReader,
        IOrderStatusReader orderStatusReader,
        ICatalogItemReader catalogItemReader,
        IPartnerReader partnerReader)
    {
        _orderRepository = orderRepository;
        _orderTypeReader = orderTypeReader;
        _orderStatusReader = orderStatusReader;
        _catalogItemReader = catalogItemReader;
        _partnerReader = partnerReader;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var partner = await _partnerReader.GetByIdAsync(command.PartnerId)
            ?? throw new KeyNotFoundException($"Partner with ID {command.PartnerId} not found.");
        var itemIds = command.OrderLines.Select(ol => ol.ItemId).Distinct();
        var itemsLookup = await _catalogItemReader.GetByIdsAsync(itemIds);

        var newOrder = Order.Create(
            partner.ToDomain(),
            command.OrderNumber,
            command.OrderDate,
            command.OrderTypeId,
            command.OrderStatusId,
            command.AdditionalInformation);

        foreach (var orderLine in command.OrderLines)
        {
            var item = itemsLookup.TryGetValue(orderLine.ItemId, out var catalogItem)
                ? catalogItem
                : throw new KeyNotFoundException($"Item with ID {orderLine.ItemId} not found");
            var newOrderLine = OrderLine.Create(
                item.ToDomain(),
                orderLine.OrderedQuantity,
                orderLine.Price.ToDomain(),
                orderLine.ExchangeRate?.ToDomain(),
                orderLine.AdditionalInformation);
            newOrder.AddOrderLine(newOrderLine);
        }

        var orderType = await _orderTypeReader.GetByIdAsync(command.OrderTypeId)
            ?? throw new InvalidOperationException($"Order Type with ID {command.OrderTypeId} not found.");

        var orderStatus = await _orderStatusReader.GetByIdAsync(command.OrderStatusId)
            ?? throw new InvalidOperationException($"Order Status with ID {command.OrderStatusId} not found.");

        return (await _orderRepository.AddAsync(newOrder)).ToDto(orderType, orderStatus);
    }
}
