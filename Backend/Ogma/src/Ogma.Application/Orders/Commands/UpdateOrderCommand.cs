using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Commands;

public record UpdateOrderCommand(
    long Id,
    long PartnerId,
    string OrderNumber,
    DateTime OrderDate,
    long OrderTypeId,
    long OrderStatusId,
    string? AdditionalInformation,
    IReadOnlyCollection<UpdateOrderLineDto> OrderLines
) : IRequest<OrderDto>;
