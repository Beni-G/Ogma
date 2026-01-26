using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Commands;

public record CreateOrderStatusCommand(string Name, string Description) : IRequest<OrderStatusDto>;
