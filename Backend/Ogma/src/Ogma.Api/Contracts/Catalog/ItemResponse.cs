using Ogma.Api.Contracts.SharedKernel;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Api.Contracts.Catalog;

public record ItemResponse(long Id,
    string Name,
    string Code,
    string Description,
    CategoryWithAncestorsResponse Category,
    MoneyResponse ListPrice,
    ItemTypeResponse ItemTypeId,
    string UnitOfMeasurement,
    bool IsActive);

