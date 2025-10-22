using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Commands;
public class UpdateItemTypeHandler : IRequestHandler<UpdateItemTypeCommand, ItemTypeDto>
{
    private readonly IItemTypeRepository _itemTypeRepository;
    public UpdateItemTypeHandler(IItemTypeRepository itemTypeRepository)
    {
        _itemTypeRepository = itemTypeRepository;
    }

    public async Task<ItemTypeDto> Handle(UpdateItemTypeCommand command, CancellationToken cancellationToken)
    {
        var existingItemType = await _itemTypeRepository.GetByIdAsync(command.Id);
        if (existingItemType == null)
        {
            throw new KeyNotFoundException($"ItemType with ID {command.Id} was not found.");
        }

        existingItemType.Update(command.Name, command.Description);

        var result = await _itemTypeRepository.UpdateAsync(existingItemType);
        if (!result)
        {
            throw new InvalidOperationException($"Update failed for ItemType with ID {command.Id}.");
        }

        return existingItemType.ToDto();
    }
}
