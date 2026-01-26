using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Queries;

public record GetOrderByIdQuery(long Id) : IRequest<OrderDto>;
