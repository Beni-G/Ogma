using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;

namespace Ogma.Application.Orders.Queries;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderReader _orderReader;
    public GetOrderByIdHandler(IOrderReader orderReader) => _orderReader = orderReader;

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderReader.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Order with Id {request.Id} was not found.");
    }
}
