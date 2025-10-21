namespace Ogma.Api.Contracts.Catalog;

public record UpdateItemRequest(long Id,
    string Name,
    string Code,
    long CategoryId,
    MoneyRequest ListPrice,
    long ItemTypeId,
    string UnitOfMeasurement,
    bool IsActive,
    string Description = "");