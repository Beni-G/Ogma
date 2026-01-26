using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;

namespace Ogma.Application.Orders.Queries;

public class GetOrderStatusByIdHandler : IRequestHandler<GetOrderStatusByIdQuery, OrderStatusDto>
{
    private readonly IOrderStatusReader _orderStatusReader;

    public GetOrderStatusByIdHandler(IOrderStatusReader orderStatusReader) => _orderStatusReader = orderStatusReader;

    public async Task<OrderStatusDto> Handle(GetOrderStatusByIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderStatusReader.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Order status with ID {request.Id} not found.");
    }
}
