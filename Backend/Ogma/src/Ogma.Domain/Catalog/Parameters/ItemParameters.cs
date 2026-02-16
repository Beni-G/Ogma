using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Catalog.Parameters;

/// <summary>
/// Represents the parameters required to define an item in the system.
/// </summary>
/// <remarks>This record encapsulates the essential details of an item, including its name, code, description, 
/// category, pricing, type, unit of measurement, and active status. It is typically used to create or  update item
/// definitions in the system.</remarks>
/// <param name="Name"></param>
/// <param name="Code"></param>
/// <param name="Category"></param>
/// <param name="ListPrice"></param>
/// <param name="ItemType"></param>
/// <param name="UnitOfMeasurement"></param>
/// <param name="IsActive"></param>
/// /// <param name="Description"></param>
public record ItemParameters(
    string Name,
    string Code,
    long CategoryId,
    Money ListPrice,
    long ItemTypeId,
    string UnitOfMeasurement,
    bool IsActive = true,
    string Description = ""
);