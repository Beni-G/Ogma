using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, OrderStatusDto>
{
    private readonly IOrderStatusRepository _orderStatusRepository;

    public UpdateOrderStatusHandler(IOrderStatusRepository orderStatusRepository) => _orderStatusRepository = orderStatusRepository;

    public async Task<OrderStatusDto> Handle(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var existingOrderStatus = await _orderStatusRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Order status with ID {command.Id} not found.");

        existingOrderStatus.Update(command.Name, command.Description);

        var result = await _orderStatusRepository.UpdateAsync(existingOrderStatus);

        if (!result)
        {
            throw new InvalidOperationException($"Update failed for order type with ID {command.Id}.");
        }

        return existingOrderStatus.ToDto();
    }
}
