using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Commands;

public record CreateOrderTypeCommand(string Code, string Description) : IRequest<OrderTypeDto>;
