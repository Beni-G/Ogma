using Ogma.Application.Catalog.Dtos;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Catalog.Entities;

namespace Ogma.Application.Catalog.Extensions;

public static class CatalogMappingExtensions
{
    /// <summary>
    /// Converts an <see cref="ItemType"/> entity to its corresponding <see cref="ItemTypeDto"/> data transfer object.
    /// </summary>
    /// <param name="entity">The <see cref="ItemType"/> entity to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="ItemTypeDto"/> instance containing the identifier, name, and description from the specified
    /// entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is <see langword="null"/>.</exception>
    public static ItemTypeDto ToDto(this ItemType entity) =>
        entity == null
        ? throw new ArgumentNullException(nameof(entity))
        : new ItemTypeDto(entity.Id, entity.Name, entity.Description);

    /// <summary>
    /// Converts a <see cref="Category"/> entity to a <see cref="CategoryDto"/> instance, optionally including ancestor
    /// and subcategory information.
    /// </summary>
    /// <param name="entity">The <see cref="Category"/> entity to convert. Cannot be null.</param>
    /// <param name="ancestors">An optional collection of ancestor categories to include in the resulting DTO. If null, an empty list is used.</param>
    /// <param name="subCategories">An optional collection of subcategory DTOs to include in the resulting DTO. If null, an empty list is used.</param>
    /// <returns>A <see cref="CategoryDto"/> representing the specified category, including any provided ancestors and
    /// subcategories.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
    public static CategoryDto ToDto(this Category entity, IEnumerable<CategoryDto>? ancestors = null, IEnumerable<CategoryDto>? subCategories = null) =>
        entity == null
        ? throw new ArgumentNullException(nameof(entity))
        : new CategoryDto(
            entity.Id,
            entity.Name,
            entity.ParentCategoryId,
            entity.Path,
            ancestors?.ToList() ?? [],
            subCategories?.ToList() ?? []
        );

    /// <summary>
    /// Returns a new CategoryDto instance with updated ancestor and subcategory collections.
    /// </summary>
    /// <param name="entity">The CategoryDto entity to enrich. Cannot be null.</param>
    /// <param name="ancestors">An optional collection of ancestor categories to assign to the entity. If null, the existing ancestors are
    /// preserved.</param>
    /// <param name="subCategories">An optional collection of subcategories to assign to the entity. If null, the existing subcategories are
    /// preserved.</param>
    /// <returns>A new CategoryDto instance with the specified ancestors and subcategories. If both ancestors and subCategories
    /// are null, returns the original entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if entity is null.</exception>
    public static CategoryDto ToEnrichedDto(this CategoryDto entity, IEnumerable<CategoryDto>? ancestors = null, IEnumerable<CategoryDto>? subCategories = null)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (ancestors == null && subCategories == null)
        {
            return entity;
        }

        return entity with
        {
            Ancestors = ancestors?.ToList() ?? entity.Ancestors,
            SubCategories = subCategories?.ToList() ?? entity.SubCategories
        };
    }

    /// <summary>
    /// Converts an <see cref="Item"/> entity to its corresponding <see cref="ItemDto"/> data transfer object.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="category"></param>
    /// <param name="itemType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static ItemDto ToDto(this Item entity, CategoryDto? category = null, ItemTypeDto? itemType = null)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        return new ItemDto(
        entity.Id,
        entity.Name,
        entity.Code,
        entity.Description,
        entity.CategoryId,
        category,
        entity.ListPrice.ToDto(),
        entity.ItemTypeId,
        itemType,
        entity.UnitOfMeasurement,
        entity.IsActive
    );
    }

}
