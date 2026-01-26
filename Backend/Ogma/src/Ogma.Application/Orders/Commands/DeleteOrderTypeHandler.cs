using MediatR;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class DeleteOrderTypeHandler : IRequestHandler<DeleteOrderTypeCommand>
{
    private readonly IOrderTypeRepository _orderTypeRepository;

    public DeleteOrderTypeHandler(IOrderTypeRepository orderTypeRepository) => _orderTypeRepository = orderTypeRepository;

    public async Task Handle(DeleteOrderTypeCommand command, CancellationToken cancellationToken)
    {
        var existingOrderType = await _orderTypeRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"OrderType with ID {command.Id} was not found.");

        await _orderTypeRepository.DeleteAsync(existingOrderType);
    }
}
