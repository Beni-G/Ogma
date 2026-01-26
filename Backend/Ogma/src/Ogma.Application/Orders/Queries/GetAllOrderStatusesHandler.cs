using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;

namespace Ogma.Application.Orders.Queries;

public class GetAllOrderStatusesHandler : IRequestHandler<GetAllOrderStatusesQuery, List<OrderStatusDto>>
{
    private readonly IOrderStatusReader _orderStatusReader;

    public GetAllOrderStatusesHandler(IOrderStatusReader orderStatusReader) => _orderStatusReader = orderStatusReader;

    public async Task<List<OrderStatusDto>> Handle(GetAllOrderStatusesQuery request, CancellationToken cancellationToken) =>
        (await _orderStatusReader.GetAllAsync()).ToList();

}
