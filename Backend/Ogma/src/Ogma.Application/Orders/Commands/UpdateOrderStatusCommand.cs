using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Commands;

public record UpdateOrderStatusCommand(long Id, string Name, string Description) : IRequest<OrderStatusDto>;
