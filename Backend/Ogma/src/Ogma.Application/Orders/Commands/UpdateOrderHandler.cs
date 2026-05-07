using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Orders.Commands;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReader _orderReader;
    private readonly IOrderTypeReader _orderTypeReader;
    private readonly IOrderStatusReader _orderStatusReader;
    private readonly ICatalogItemReader _catalogItemReader;
    private readonly IPartnerReader _partnerReader;

    public UpdateOrderHandler(
        IOrderRepository orderRepository,
        IOrderReader orderReader,
        IOrderTypeReader orderTypeReader,
        IOrderStatusReader orderStatusReader,
        ICatalogItemReader catalogItemReader,
        IPartnerReader partnerReader)
    {
        _orderRepository = orderRepository;
        _orderReader = orderReader;
        _orderTypeReader = orderTypeReader;
        _orderStatusReader = orderStatusReader;
        _catalogItemReader = catalogItemReader;
        _partnerReader = partnerReader;
    }
    public async Task<OrderDto> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Order with ID {command.Id} not found.");
        var partner = await _partnerReader.GetByIdAsync(command.PartnerId)
            ?? throw new KeyNotFoundException($"Partner with ID {command.PartnerId} not found.");
        _ = await _orderTypeReader.GetByIdAsync(command.OrderTypeId)
            ?? throw new KeyNotFoundException($"Order Type with ID {command.OrderTypeId} not found.");
        _ = await _orderStatusReader.GetByIdAsync(command.OrderStatusId)
            ?? throw new KeyNotFoundException($"Order Status with ID {command.OrderStatusId} not found.");
        var itemIds = command.OrderLines.Select(ol => ol.ItemId).Distinct();
        var itemsLookup = await _catalogItemReader.GetByIdsAsync(itemIds);

        var orderLineInputs = command.OrderLines.Select(orderLine =>
        {
            var item = itemsLookup.TryGetValue(orderLine.ItemId, out var catalogItem)
            ? catalogItem
            : throw new KeyNotFoundException($"Item with ID {orderLine.ItemId} not found");

            return new OrderLineInput(
                orderLine.Id,
                new OrderItem(item.ItemId, item.ItemName, item.ItemCode),
                orderLine.OrderedQuantity,
                orderLine.CancelledQuantity,
                orderLine.FullfilledQuantity,
                new Money(orderLine.Price.Amount, orderLine.Price.Currency),
                orderLine.ExchangeRate != null
                    ? new ExchangeRate(orderLine.ExchangeRate.BaseCurrency, orderLine.ExchangeRate.TargetCurrency, orderLine.ExchangeRate.Rate)
                    : null,
                orderLine.AdditionalInformation);
        });

        existingOrder.Update(
                new OrderPartner(command.PartnerId, partner.PartnerName),
                command.OrderNumber,
                command.OrderDate,
                command.OrderTypeId,
                command.OrderStatusId,
                command.AdditionalInformation!,
                orderLineInputs);

        var result = await _orderRepository.UpdateAsync(existingOrder);
        if (!result)
        {
            throw new InvalidOperationException($"Failed to update Order with ID {command.Id}.");
        }

        return await _orderReader.GetByIdAsync(command.Id)
            ?? throw new InvalidOperationException($"Failed to retrieve updated Order with ID {command.Id}.");

    }
}
