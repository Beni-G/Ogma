using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Catalog;

public record ItemResponse(long Id,
    string Name,
    string Code,
    string Description,
    long CategoryId,
    CategoryResponse Category,
    MoneyResponse ListPrice,
    long ItemTypeId,
    ItemTypeResponse ItemType,
    string UnitOfMeasurement,
    bool IsActive);

