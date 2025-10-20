using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Queries;
public class GetItemTypeByIdHandler : IRequestHandler<GetItemTypeByIdQuery, ItemTypeDto>
{
    private readonly IItemTypeRepository _itemTypeRepository;
    public GetItemTypeByIdHandler(IItemTypeRepository itemTypeRepository)
    {
        _itemTypeRepository = itemTypeRepository;
    }

    public async Task<ItemTypeDto> Handle(GetItemTypeByIdQuery query, CancellationToken cancellationToken)
    {
        var itemType = await _itemTypeRepository.GetByIdAsync(query.Id);
        if(itemType == null)
        {
            throw new KeyNotFoundException($"ItemType with ID {query.Id} was not found.");
        }
        return itemType.ToDto();
    }
}
