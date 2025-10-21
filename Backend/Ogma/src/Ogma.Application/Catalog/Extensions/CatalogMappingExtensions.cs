using Ogma.Application.Catalog.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.Catalog.Extensions;
public static class CatalogMappingExtensions
{
    public static ItemTypeDto ToDto(this ItemType entity) => new ItemTypeDto(entity.Id, entity.Name, entity.Description);

    public static CategoryDto ToDto(this Category entity) => new CategoryDto(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path);

    public static CategoryWithDescendantsDto ToDtoWithDescendants(this Category entity) => new CategoryWithDescendantsDto(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        entity.SubCategories.Select(c => c.ToDtoWithDescendants()).ToList());

    public static CategoryWithAncestorsDto ToDtoWithAncestors(this Category entity, IEnumerable<Category> ancestors = null) => new CategoryWithAncestorsDto(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        ancestors?.Select(c => c.ToDto()).ToList());

    public static ItemDto ToDto(this Item entity, IEnumerable<Category> ancestors = null) => new ItemDto(entity.Id,
        entity.Name,
        entity.Code,
        entity.Description,
        entity.Category.ToDtoWithAncestors(ancestors),
        entity.ListPrice.ToDto(),
        entity.ItemType.ToDto(),
        entity.UnitOfMeasurement,
        entity.IsActive);

    public static MoneyDto ToDto(this Money money) => new MoneyDto(money.Amount, money.Currency);
}
