using MediatR;

namespace Ogma.Application.Orders.Commands;

public record DeleteOrderStatusCommand(long Id) : IRequest;
