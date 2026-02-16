using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Application.Catalog.Ports;

namespace Ogma.Application.Catalog.Queries;

public class GetItemByIdHandler : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    private readonly IItemReader _itemReader;
    private readonly ICategoryReader _categoryReader;
    private readonly IItemTypeReader _itemTypeReader;
    public GetItemByIdHandler(IItemReader itemReader, ICategoryReader categoryReader, IItemTypeReader itemTypeReader)
    {
        _itemReader = itemReader;
        _categoryReader = categoryReader;
        _itemTypeReader = itemTypeReader;
    }
    public async Task<ItemDto> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await _itemReader.GetByIdAsync(query.Id)
            ?? throw new KeyNotFoundException($"ItemType with ID {query.Id} was not found.");

        var category = await _categoryReader.GetByIdAsync(item.CategoryId)
            ?? throw new KeyNotFoundException($"Category with ID {item.CategoryId} was not found.");
        var categoryAncestors = await _categoryReader.GetAncestorsAsync(item.CategoryId);

        var itemType = await _itemTypeReader.GetByIdAsync(item.ItemTypeId)
            ?? throw new KeyNotFoundException($"ItemType with ID {item.ItemTypeId} was not found.");

        return new ItemDto(
            item.Id,
            item.Name,
            item.Code,
            item.Description,
            item.CategoryId,
            category.ToEnrichedDto(categoryAncestors),
            item.ListPrice,
            item.ItemTypeId,
            item.ItemType,
            item.UnitOfMeasurement,
            item.IsActive
        );

    }
}
