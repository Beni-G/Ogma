using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class CreateOrderTypeHandler : IRequestHandler<CreateOrderTypeCommand, OrderTypeDto>
{
    private readonly IOrderTypeRepository _orderTypeRepository;
    public CreateOrderTypeHandler(IOrderTypeRepository orderTypeRepository) => _orderTypeRepository = orderTypeRepository;
    public async Task<OrderTypeDto> Handle(CreateOrderTypeCommand command, CancellationToken cancellationToken)
    {
        var newOrderType = OrderType.Create(command.Code, command.Description);
        var created = await _orderTypeRepository.AddAsync(newOrderType);
        return created.ToDto();
    }
}
