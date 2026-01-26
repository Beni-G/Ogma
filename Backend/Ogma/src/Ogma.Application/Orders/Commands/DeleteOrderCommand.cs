using MediatR;

namespace Ogma.Application.Orders.Commands;

public record DeleteOrderCommand(long Id) : IRequest;
