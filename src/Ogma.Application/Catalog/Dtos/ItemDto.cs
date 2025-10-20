using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Catalog.Dtos;
public record ItemDto(long Id,
    string Name, 
    string Code, 
    string Description,
    CategoryWithAncestorsDto Category, 
    MoneyDto ListPrice, 
    ItemTypeDto ItemType, 
    string UnitOfMeasurement, 
    bool IsActive);


