using MediatR;
using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Queries;

public record GetAllOrderStatusesQuery() : IRequest<List<OrderStatusDto>>;
