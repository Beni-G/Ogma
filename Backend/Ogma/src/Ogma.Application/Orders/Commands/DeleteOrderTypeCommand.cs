using MediatR;

namespace Ogma.Application.Orders.Commands;

public record DeleteOrderTypeCommand(long Id) : IRequest;
