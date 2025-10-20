using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Catalog.Commands;
public class CreateItemHandler : IRequestHandler<CreateItemCommand, ItemDto>
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemTypeRepository _itemTypeRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateItemHandler(IItemRepository itemRepository, IItemTypeRepository itemTypeRepository, ICategoryRepository categoryRepository)
    {
        _itemRepository = itemRepository;
        _itemTypeRepository = itemTypeRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ItemDto> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var listPrice = new Money(command.ListPrice.Amount, command.ListPrice.Currency);
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with Id {command.CategoryId} not found.");
        }
        var categoryAncestors = await _categoryRepository.GetAncestorsAsync(category.Id);

        var itemType = await _itemTypeRepository.GetByIdAsync(command.ItemTypeId);  

        if (itemType == null)
        {
            throw new KeyNotFoundException($"ItemType with Id {command.ItemTypeId} not found.");
        }

        var parameters = new ItemParameters(command.Name,
            command.Code,
            category,
            listPrice,
            itemType,
            command.UnitOfMeasurement,
            command.IsActive,
            command.Description);

        var newItem = Item.Create(parameters);

        return (await _itemRepository.AddAsync(newItem)).ToDto(categoryAncestors);
    }
}
