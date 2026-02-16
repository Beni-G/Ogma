using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;

namespace Ogma.Application.Catalog.Queries;

public class GetAllItemTypesHandler : IRequestHandler<GetAllItemTypesQuery, List<ItemTypeDto>>
{
    private readonly IItemTypeReader _itemTypeReader;
    public GetAllItemTypesHandler(IItemTypeReader itemTypeReader) => _itemTypeReader = itemTypeReader;

    public async Task<List<ItemTypeDto>> Handle(GetAllItemTypesQuery query, CancellationToken cancellationToken) =>
         (await _itemTypeReader.GetAllAsync()).ToList();
}
