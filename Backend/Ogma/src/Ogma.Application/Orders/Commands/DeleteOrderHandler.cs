using MediatR;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.Orders.Commands;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Order with Id {request.Id} not found.");
        await _orderRepository.DeleteAsync(order);
    }
}
