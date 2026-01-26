using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;

namespace Ogma.Application.Orders.Queries;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{
    private readonly IOrderReader _orderReader;

    public GetAllOrdersHandler(IOrderReader orderReader) => _orderReader = orderReader;

    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken) => (await _orderReader.GetAllAsync()).ToList();

}
