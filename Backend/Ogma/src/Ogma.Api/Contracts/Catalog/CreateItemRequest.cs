using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Catalog;

public record CreateItemRequest(string Name,
    string Code,
    long CategoryId,
    MoneyRequest ListPrice,
    long ItemTypeId,
    string UnitOfMeasurement,
    bool IsActive,
    string Description = "");
