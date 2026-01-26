using MediatR;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class DeleteOrderStatusHandler : IRequestHandler<DeleteOrderStatusCommand>
{
    private readonly IOrderStatusRepository _orderStatusRepository;

    public DeleteOrderStatusHandler(IOrderStatusRepository orderStatusRepository) => _orderStatusRepository = orderStatusRepository;

    public async Task Handle(DeleteOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var existingOrderStatus = await _orderStatusRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Order status with Id {request.Id} was not found.");

        await _orderStatusRepository.DeleteAsync(existingOrderStatus);
    }
}
