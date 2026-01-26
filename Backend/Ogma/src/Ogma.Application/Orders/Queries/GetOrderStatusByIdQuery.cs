using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Queries;

public record GetOrderStatusByIdQuery(long Id) : IRequest<OrderStatusDto>;
