using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Application.Catalog.Ports;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Catalog.Commands;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, ItemDto>
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemReader _itemReader;
    private readonly IItemTypeReader _itemTypeReader;
    private readonly ICategoryReader _categoryReader;

    public UpdateItemHandler(IItemRepository itemRepository, IItemReader itemReader, IItemTypeReader itemTypeReader, ICategoryReader categoryReader)
    {
        _itemRepository = itemRepository;
        _itemReader = itemReader;
        _itemTypeReader = itemTypeReader;
        _categoryReader = categoryReader;
    }

    public async Task<ItemDto> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var existingItem = await _itemRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Item with Id {command.Id} not found.");

        var category = await _categoryReader.GetByIdAsync(command.CategoryId)
            ?? throw new KeyNotFoundException($"Category with Id {command.CategoryId} not found.");
        var categoryAncestors = await _categoryReader.GetAncestorsAsync(category.Id);
        category.ToEnrichedDto(categoryAncestors);

        var itemType = await _itemTypeReader.GetByIdAsync(command.ItemTypeId)
            ?? throw new KeyNotFoundException($"ItemType with Id {command.ItemTypeId} not found.");

        var parameters = new ItemParameters(command.Name,
            command.Code,
            command.CategoryId,
            new Money(command.ListPrice.Amount, command.ListPrice.Currency),
            command.ItemTypeId,
            command.UnitOfMeasurement,
            command.IsActive,
            command.Description
        );

        existingItem.UpdateItem(parameters);
        var result = await _itemRepository.UpdateAsync(existingItem);
        if (!result)
        {
            throw new InvalidOperationException($"Update failed for ItemType with ID {command.Id}.");
        }

        var updatedItem = await _itemReader.GetByIdAsync(command.Id)
            ?? throw new InvalidOperationException($"Failed to retrieve updated Order with ID {command.Id}.");

        return new ItemDto(
            updatedItem.Id,
            updatedItem.Name,
            updatedItem.Code,
            updatedItem.Description,
            updatedItem.CategoryId,
            category,
            updatedItem.ListPrice,
            updatedItem.ItemTypeId,
            itemType,
            updatedItem.UnitOfMeasurement,
            updatedItem.IsActive
        );
    }
}
