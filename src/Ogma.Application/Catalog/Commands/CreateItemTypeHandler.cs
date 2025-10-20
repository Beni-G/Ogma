using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Commands;
public class CreateItemTypeHandler : IRequestHandler<CreateItemTypeCommand, ItemTypeDto>
{
    private readonly IItemTypeRepository _itemTypeRepository;
    public CreateItemTypeHandler(IItemTypeRepository itemTypeRepository)
    {
        _itemTypeRepository = itemTypeRepository;
    }

    public async Task<ItemTypeDto> Handle(CreateItemTypeCommand request, CancellationToken cancellationToken)
    {
        var newItemType = ItemType.Create(request.Name, request.Description);
        var created = await _itemTypeRepository.AddAsync(newItemType);
        return created.ToDto();
    }
}
