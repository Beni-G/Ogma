using Ogma.Api.Contracts.Catalog;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Api.Extensions;

public static class CatalogMappingExtensions
{
    public static ItemTypeResponse ToResponse(this ItemType entity) => new ItemTypeResponse(entity.Id, entity.Name, entity.Description);
    public static ItemTypeResponse ToResponse(this ItemTypeDto entity) => new ItemTypeResponse(entity.Id, entity.Name, entity.Description);

    public static CategoryWithDescendantsResponse ToResponseWithDescendants(this Category entity) => new CategoryWithDescendantsResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        entity.SubCategories.Select(c => c.ToResponseWithDescendants()).ToList());
    public static CategoryWithDescendantsResponse ToResponseWithDescendants(this CategoryWithDescendantsDto entity) => new CategoryWithDescendantsResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        entity.SubCategories.Select(c => c.ToResponseWithDescendants()).ToList());
    public static CategoryResponse ToResponse(this Category entity) => new CategoryResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path);
    public static CategoryResponse ToResponse(this CategoryDto entity) => new CategoryResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path);

    public static CategoryWithAncestorsResponse ToResponseWithAncestors(this Category entity) => new CategoryWithAncestorsResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        new List<CategoryResponse>());
    public static CategoryWithAncestorsResponse ToResponseWithAncestors(this CategoryWithAncestorsDto entity) => new CategoryWithAncestorsResponse(
        entity.Id,
        entity.Name,
        entity.ParentCategoryId,
        entity.Path,
        entity.Ancestors?.Select(c => c.ToResponse()).ToList());

    public static ItemResponse ToResponse(this Item entity) => new ItemResponse(entity.Id,
        entity.Name,
        entity.Code,
        entity.Description,
        entity.Category.ToResponseWithAncestors(),
        entity.ListPrice.ToResponse(),
        entity.ItemType.ToResponse(),
        entity.UnitOfMeasurement,
        entity.IsActive);

    public static ItemResponse ToResponse(this ItemDto entity) => new ItemResponse(entity.Id,
        entity.Name,
        entity.Code,
        entity.Description,
        entity.Category.ToResponseWithAncestors(),
        entity.ListPrice.ToResponse(),
        entity.ItemType.ToResponse(),
        entity.UnitOfMeasurement,
        entity.IsActive);

    public static MoneyResponse ToResponse(this Money money) => new MoneyResponse(money.Amount, money.Currency);

    public static MoneyResponse ToResponse(this MoneyDto money) => new MoneyResponse(money.Amount, money.Currency);

}
