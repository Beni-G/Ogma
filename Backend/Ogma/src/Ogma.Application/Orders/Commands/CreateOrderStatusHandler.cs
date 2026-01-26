using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class CreateOrderStatusHandler : IRequestHandler<CreateOrderStatusCommand, OrderStatusDto>
{
    private readonly IOrderStatusRepository _orderStatusRepository;

    public CreateOrderStatusHandler(IOrderStatusRepository orderStatusRepository) => _orderStatusRepository = orderStatusRepository;

    public async Task<OrderStatusDto> Handle(CreateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var newOrderStatus = OrderStatus.Create(command.Name, command.Description);
        var created = await _orderStatusRepository.AddAsync(newOrderStatus);
        return created.ToDto();
    }
}
