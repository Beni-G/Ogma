using Ogma.Api.Contracts.Catalog;
using Ogma.Application.Catalog.Dtos;
using Ogma.Domain.Catalog.Entities;

namespace Ogma.Api.Extensions;

public static class CatalogMappingExtensions
{
    public static ItemTypeResponse ToResponse(this ItemType entity) => new ItemTypeResponse(entity.Id, entity.Name, entity.Description);
    public static ItemTypeResponse ToResponse(this ItemTypeDto entity) => new ItemTypeResponse(entity.Id, entity.Name, entity.Description);
    public static CategoryResponse ToResponse(this CategoryDto entity) => new CategoryResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        entity.Ancestors?.Select(c => c.ToResponse()).ToList(),
        entity.SubCategories?.Select(c => c.ToResponse()).ToList()
    );
    public static ItemResponse ToResponse(this ItemDto entity) => new ItemResponse(entity.Id,
        entity.Name,
        entity.Code,
        entity.Description,
        entity.CategoryId,
        entity.Category!.ToResponse(),
        entity.ListPrice.ToResponse(),
        entity.ItemTypeId,
        entity.ItemType!.ToResponse(),
        entity.UnitOfMeasurement,
        entity.IsActive);


}
