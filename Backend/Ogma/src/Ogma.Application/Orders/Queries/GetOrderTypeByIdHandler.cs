using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;

namespace Ogma.Application.Orders.Queries;

public class GetOrderTypeByIdHandler : IRequestHandler<GetOrderTypeByIdQuery, OrderTypeDto>
{
    private readonly IOrderTypeReader _orderTypeReader;

    public GetOrderTypeByIdHandler(IOrderTypeReader orderTypeReader) => _orderTypeReader = orderTypeReader;

    public async Task<OrderTypeDto> Handle(GetOrderTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderTypeReader.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Order type with Id {request.Id} not found.");
    }
}
