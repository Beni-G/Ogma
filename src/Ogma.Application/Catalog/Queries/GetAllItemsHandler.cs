using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Queries;
public class GetAllItemsHandler : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
{
    private readonly IItemRepository _itemRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllItemsHandler(IItemRepository itemRepository, ICategoryRepository categoryRepository)
    {
        _itemRepository = itemRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ItemDto>> Handle(GetAllItemsQuery query, CancellationToken cancellationToken)
    {
        var itemDtos = new List<ItemDto>();
        var items = await _itemRepository.GetAllAsync();

        foreach (var item in items)
        {
            var categoryAncestors = await _categoryRepository.GetAncestorsAsync(item.Category.Id);
            var itemDto = item.ToDto(categoryAncestors);
            itemDtos.Add(itemDto);
        }

        return itemDtos;
    }
}
