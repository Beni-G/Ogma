using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Queries;
public class GetItemByIdHandler : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    private readonly IItemRepository _itemRepository;
    private readonly ICategoryRepository _categoryRepository;
    public GetItemByIdHandler(IItemRepository itemRepository, ICategoryRepository categoryRepository)
    {
        _itemRepository = itemRepository;
        _categoryRepository = categoryRepository;
    }
    public async Task<ItemDto> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetByIdAsync(query.Id);
        if (item == null)
        {
            throw new KeyNotFoundException($"ItemType with ID {query.Id} was not found.");
        }

        var categoryAncestors = await _categoryRepository.GetAncestorsAsync(item.Category.Id);

        return item.ToDto(categoryAncestors);

    }
}
