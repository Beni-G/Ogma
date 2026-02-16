using MediatR;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Commands;
public class DeleteItemHandler : IRequestHandler<DeleteItemCommand>
{
    private readonly IItemRepository _itemRepository;
    public DeleteItemHandler(IItemRepository itemTypeRepository) => _itemRepository = itemTypeRepository;

    public async Task Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var existing = await _itemRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Item with ID {command.Id} was not found.");
        await _itemRepository.DeleteAsync(existing);
    }
}
