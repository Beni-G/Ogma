using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;

namespace Ogma.Application.Catalog.Queries;

public class GetItemTypeByIdHandler : IRequestHandler<GetItemTypeByIdQuery, ItemTypeDto>
{
    private readonly IItemTypeReader _itemTypeReader;
    public GetItemTypeByIdHandler(IItemTypeReader itemTypeReader) => _itemTypeReader = itemTypeReader;

    public async Task<ItemTypeDto> Handle(GetItemTypeByIdQuery query, CancellationToken cancellationToken)
    {
        var itemType = await _itemTypeReader.GetByIdAsync(query.Id)
            ?? throw new KeyNotFoundException($"ItemType with ID {query.Id} was not found.");
        return itemType;
    }
}
