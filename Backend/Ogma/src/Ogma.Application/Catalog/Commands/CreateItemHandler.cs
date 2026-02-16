using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Application.Catalog.Ports;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Catalog.Commands;
public class CreateItemHandler : IRequestHandler<CreateItemCommand, ItemDto>
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemTypeReader _itemTypeReader;
    private readonly ICategoryReader _categoryReader;

    public CreateItemHandler(IItemRepository itemRepository, IItemTypeReader itemTypeReader, ICategoryReader categoryReader)
    {
        _itemRepository = itemRepository;
        _itemTypeReader = itemTypeReader;
        _categoryReader = categoryReader;
    }

    public async Task<ItemDto> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryReader.GetByIdAsync(command.CategoryId)
            ?? throw new KeyNotFoundException($"Category with Id {command.CategoryId} not found.");

        var itemType = await _itemTypeReader.GetByIdAsync(command.ItemTypeId)
            ?? throw new KeyNotFoundException($"ItemType with Id {command.ItemTypeId} not found.");

        var parameters = new ItemParameters(
            command.Name,
            command.Code,
            command.CategoryId,
            new Money(command.ListPrice.Amount, command.ListPrice.Currency),
            command.ItemTypeId,
            command.UnitOfMeasurement,
            command.IsActive,
            command.Description
        );

        var newItem = Item.Create(parameters);
        var created = await _itemRepository.AddAsync(newItem);
        return created.ToDto(category, itemType);
    }
}
