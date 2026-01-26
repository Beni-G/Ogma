using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;

namespace Ogma.Application.Orders.Queries;

public class GetAllOrderTypesHandler : IRequestHandler<GetAllOrderTypesQuery, List<OrderTypeDto>>
{
    private readonly IOrderTypeReader _orderTypeReader;

    public GetAllOrderTypesHandler(IOrderTypeReader orderTypeReader) => _orderTypeReader = orderTypeReader;

    public async Task<List<OrderTypeDto>> Handle(GetAllOrderTypesQuery request, CancellationToken cancellationToken) =>
        (await _orderTypeReader.GetAllAsync()).ToList();
}
