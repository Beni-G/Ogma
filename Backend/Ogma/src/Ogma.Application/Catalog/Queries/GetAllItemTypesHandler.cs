using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Queries;
public class GetAllItemTypesHandler : IRequestHandler<GetAllItemTypesQuery, List<ItemTypeDto>>
{
    private readonly IItemTypeRepository _itemTypeRepository;
    public GetAllItemTypesHandler(IItemTypeRepository itemTypeRepository)
    {
        _itemTypeRepository = itemTypeRepository;
    }

    public async Task<List<ItemTypeDto>> Handle(GetAllItemTypesQuery query, CancellationToken cancellationToken)
    {
        var itemTypes = await _itemTypeRepository.GetAllAsync();
        return itemTypes.Select(it => it.ToDto()).ToList();
    }
}
