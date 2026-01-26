using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Commands;

public record CreateOrderCommand(
    long PartnerId,
    string OrderNumber,
    DateTime OrderDate,
    long OrderTypeId,
    long OrderStatusId,
    string? AdditionalInformation,
    IReadOnlyCollection<CreateOrderLineDto> OrderLines) : IRequest<OrderDto>;

