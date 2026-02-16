using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Application.Catalog.Ports;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Catalog.Queries;

public class GetAllItemsHandler : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
{
    private readonly IItemReader _itemReader;
    private readonly ICategoryReader _categoryReader;

    public GetAllItemsHandler(IItemReader itemReader, ICategoryReader categoryReader)
    {
        _itemReader = itemReader;
        _categoryReader = categoryReader;
    }

    public async Task<List<ItemDto>> Handle(GetAllItemsQuery query, CancellationToken cancellationToken)
    {
        var itemDtos = new List<ItemDto>();
        var items = await _itemReader.GetAllAsync();
        var categoryIds = items.Select(i => i.CategoryId).Distinct();
        var categoriesLookup = await _categoryReader.GetByIdsAsync(categoryIds);
        var idsThatNeedAncestors = categoriesLookup.Values
            .Where(c => c.ParentCategoryId.HasValue)
            .Select(c => c.Id)
            .Distinct();
        var ancestorsLookup = await _categoryReader.GetAncestorsAsync(idsThatNeedAncestors);

        foreach (var item in items)
        {
            if (!categoriesLookup.TryGetValue(item.CategoryId, out var category))
            {
                throw new KeyNotFoundException($"Category with ID {item.CategoryId} not found.");
            }
            ancestorsLookup.TryGetValue(item.CategoryId, out var categoryAncestors);
            var ancestors = categoryAncestors ?? [];
            var itemDto = new ItemDto(
                item.Id,
                item.Name,
                item.Code,
                item.Description,
                item.CategoryId,
                category.ToEnrichedDto(ancestors),
                new MoneyDto(item.ListPrice.Amount, item.ListPrice.Currency),
                item.ItemTypeId,
                item.ItemType,
                item.UnitOfMeasurement,
                item.IsActive);
            itemDtos.Add(itemDto);
        }

        return itemDtos;
    }
}
