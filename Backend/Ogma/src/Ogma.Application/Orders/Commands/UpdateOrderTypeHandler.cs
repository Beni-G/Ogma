using MediatR;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class UpdateOrderTypeHandler : IRequestHandler<UpdateOrderTypeCommand, OrderTypeDto>
{
    private readonly IOrderTypeRepository _orderTypeRepository;

    public UpdateOrderTypeHandler(IOrderTypeRepository orderTypeRepository) => _orderTypeRepository = orderTypeRepository;

    public async Task<OrderTypeDto> Handle(UpdateOrderTypeCommand command, CancellationToken cancellationToken)
    {
        var existingOrderType = await _orderTypeRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Ordert type with ID {command.Id} was not found.");

        existingOrderType.Update(command.Code, command.Description);

        var result = await _orderTypeRepository.UpdateAsync(existingOrderType);

        if (!result)
        {
            throw new InvalidOperationException($"Update failed for order type with ID {command.Id}.");
        }

        return existingOrderType.ToDto();
    }
}
