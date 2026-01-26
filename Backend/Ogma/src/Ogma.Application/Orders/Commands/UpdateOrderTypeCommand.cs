using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Commands;

public record UpdateOrderTypeCommand(long Id, string Code, string Description) : IRequest<OrderTypeDto>;
