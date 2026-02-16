using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Catalog.Dtos;
public record ItemDto(long Id,
    string Name, 
    string Code, 
    string Description,
    long CategoryId,
    CategoryDto? Category, 
    MoneyDto ListPrice, 
    long ItemTypeId,
    ItemTypeDto? ItemType, 
    string UnitOfMeasurement, 
    bool IsActive);


