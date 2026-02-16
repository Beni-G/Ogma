using MediatR;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Commands;
public class DeleteItemTypeHandler : IRequestHandler<DeleteItemTypeCommand>
{
    private readonly IItemTypeRepository _itemTypeRepository;
    public DeleteItemTypeHandler(IItemTypeRepository itemTypeRepository) => _itemTypeRepository = itemTypeRepository;

    public async Task Handle(DeleteItemTypeCommand command, CancellationToken cancellationToken)
    {
        var existing = await _itemTypeRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"ItemType with ID {command.Id} was not found.");
        await _itemTypeRepository.DeleteAsync(existing);
    }
}
